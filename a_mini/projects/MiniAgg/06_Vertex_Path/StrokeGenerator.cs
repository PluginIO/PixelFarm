//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2002-2005 Maxim Shemanarev (http://www.antigrain.com)
//
// C# Port port by: Lars Brubaker
//                  larsbrubaker@gmail.com
// Copyright (C) 2007
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
//----------------------------------------------------------------------------
// Contact: mcseem@antigrain.com
//          mcseemagg@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------
using System;
using MatterHackers.VectorMath;

namespace MatterHackers.Agg.VertexSource
{
    public class Vector2Container : ArrayList<Vector2>, IVertexDest
    {

    }

    //============================================================vcgen_stroke
    class StrokeGenerator : IGenerator
    {
        StrokeMath m_stroker;

        VertexSequence m_src_vertices;
        Vector2Container m_out_vertices;

        double m_shorten;
        int m_closed;
        StrokeMath.Status m_status;
        StrokeMath.Status m_prev_status;

        int m_src_vertex;
        int m_out_vertex;

        public StrokeGenerator()
        {
            m_stroker = new StrokeMath();
            m_src_vertices = new VertexSequence();
            m_out_vertices = new Vector2Container();
            m_status = StrokeMath.Status.Init;
        }

        public void line_cap(LineCap lc) { m_stroker.line_cap(lc); }
        public void line_join(LineJoin lj) { m_stroker.line_join(lj); }
        public void inner_join(InnerJoin ij) { m_stroker.inner_join(ij); }

        public LineCap line_cap() { return m_stroker.line_cap(); }
        public LineJoin line_join() { return m_stroker.line_join(); }
        public InnerJoin inner_join() { return m_stroker.inner_join(); }

        public void width(double w) { m_stroker.width(w); }
        public void miter_limit(double ml) { m_stroker.miter_limit(ml); }
        public void miter_limit_theta(double t) { m_stroker.miter_limit_theta(t); }
        public void inner_miter_limit(double ml) { m_stroker.inner_miter_limit(ml); }
        public void approximation_scale(double approx_scale) { m_stroker.approximation_scale(approx_scale); }

        public double width() { return m_stroker.width(); }
        public double miter_limit() { return m_stroker.miter_limit(); }
        public double inner_miter_limit() { return m_stroker.inner_miter_limit(); }
        public double approximation_scale() { return m_stroker.approximation_scale(); }

        public void auto_detect_orientation(bool v) { throw new Exception(); }
        public bool auto_detect_orientation() { throw new Exception(); }

        public void shorten(double s) { m_shorten = s; }
        public double shorten() { return m_shorten; }

        // Vertex Generator Interface
        public void RemoveAll()
        {
            m_src_vertices.Clear();
            m_closed = 0;
            m_status = StrokeMath.Status.Init;
        }

        public void AddVertex(double x, double y, ShapePath.FlagsAndCommand cmd)
        {
            m_status = StrokeMath.Status.Init;
            if (ShapePath.is_move_to(cmd))
            {
                m_src_vertices.ReplaceLast(new VertexDistance(x, y));
            }
            else
            {
                if (ShapePath.IsVertextCommand(cmd))
                {
                    m_src_vertices.AddVertex(new VertexDistance(x, y));
                }
                else
                {
                    m_closed = (int)ShapePath.get_close_flag(cmd);
                }
            }
        }

        // Vertex Source Interface
        public void Rewind(int idx)
        {
            if (m_status == StrokeMath.Status.Init)
            {
                m_src_vertices.Close(m_closed != 0);
                ShapePath.shorten_path(m_src_vertices, m_shorten, m_closed);
                if (m_src_vertices.Count < 3) m_closed = 0;
            }
            m_status = StrokeMath.Status.Ready;
            m_src_vertex = 0;
            m_out_vertex = 0;
        }

        public ShapePath.FlagsAndCommand Vertex(ref double x, ref double y)
        {
            ShapePath.FlagsAndCommand cmd = ShapePath.FlagsAndCommand.CommandLineTo;
            while (!ShapePath.is_stop(cmd))
            {
                switch (m_status)
                {
                    case StrokeMath.Status.Init:
                        Rewind(0);
                        goto case StrokeMath.Status.Ready;

                    case StrokeMath.Status.Ready:

                        if (m_src_vertices.Count < 2 + (m_closed != 0 ? 1 : 0))
                        {
                            cmd = ShapePath.FlagsAndCommand.CommandStop;
                            break;
                        }
                        m_status = (m_closed != 0) ? StrokeMath.Status.Outline1 : StrokeMath.Status.Cap1;
                        cmd = ShapePath.FlagsAndCommand.CommandMoveTo;
                        m_src_vertex = 0;
                        m_out_vertex = 0;
                        break;

                    case StrokeMath.Status.Cap1:
                        m_stroker.calc_cap(m_out_vertices, m_src_vertices[0], m_src_vertices[1],
                            m_src_vertices[0].dist);
                        m_src_vertex = 1;
                        m_prev_status = StrokeMath.Status.Outline1;
                        m_status = StrokeMath.Status.OutVertices;
                        m_out_vertex = 0;
                        break;

                    case StrokeMath.Status.Cap2:
                        m_stroker.calc_cap(m_out_vertices,
                            m_src_vertices[m_src_vertices.Count - 1],
                            m_src_vertices[m_src_vertices.Count - 2],
                            m_src_vertices[m_src_vertices.Count - 2].dist);
                        m_prev_status = StrokeMath.Status.Outline2;
                        m_status = StrokeMath.Status.OutVertices;
                        m_out_vertex = 0;
                        break;

                    case StrokeMath.Status.Outline1:
                        if (m_closed != 0)
                        {
                            if (m_src_vertex >= m_src_vertices.Count)
                            {
                                m_prev_status = StrokeMath.Status.CloseFirst;
                                m_status = StrokeMath.Status.EndPoly1;
                                break;
                            }
                        }
                        else
                        {
                            if (m_src_vertex >= m_src_vertices.Count - 1)
                            {
                                m_status = StrokeMath.Status.Cap2;
                                break;
                            }
                        }
                        m_stroker.calc_join(m_out_vertices,
                            m_src_vertices.prev(m_src_vertex),
                            m_src_vertices.curr(m_src_vertex),
                            m_src_vertices.next(m_src_vertex),
                            m_src_vertices.prev(m_src_vertex).dist,
                            m_src_vertices.curr(m_src_vertex).dist);
                        ++m_src_vertex;
                        m_prev_status = m_status;
                        m_status = StrokeMath.Status.OutVertices;
                        m_out_vertex = 0;
                        break;

                    case StrokeMath.Status.CloseFirst:
                        m_status = StrokeMath.Status.Outline2;
                        cmd = ShapePath.FlagsAndCommand.CommandMoveTo;
                        goto case StrokeMath.Status.Outline2;

                    case StrokeMath.Status.Outline2:
                        if (m_src_vertex <= (m_closed == 0 ? 1 : 0))
                        {
                            m_status = StrokeMath.Status.EndPoly2;
                            m_prev_status = StrokeMath.Status.Stop;
                            break;
                        }

                        --m_src_vertex;
                        m_stroker.calc_join(m_out_vertices,
                            m_src_vertices.next(m_src_vertex),
                            m_src_vertices.curr(m_src_vertex),
                            m_src_vertices.prev(m_src_vertex),
                            m_src_vertices.curr(m_src_vertex).dist,
                            m_src_vertices.prev(m_src_vertex).dist);

                        m_prev_status = m_status;
                        m_status = StrokeMath.Status.OutVertices;
                        m_out_vertex = 0;
                        break;

                    case StrokeMath.Status.OutVertices:
                        if (m_out_vertex >= m_out_vertices.Count)
                        {
                            m_status = m_prev_status;
                        }
                        else
                        {
                            Vector2 c = m_out_vertices[(int)m_out_vertex++];
                            x = c.x;
                            y = c.y;
                            return cmd;
                        }
                        break;

                    case StrokeMath.Status.EndPoly1:
                        m_status = m_prev_status;
                        return ShapePath.FlagsAndCommand.CommandEndPoly
                            | ShapePath.FlagsAndCommand.FlagClose
                            | ShapePath.FlagsAndCommand.FlagCCW;

                    case StrokeMath.Status.EndPoly2:
                        m_status = m_prev_status;
                        return ShapePath.FlagsAndCommand.CommandEndPoly
                            | ShapePath.FlagsAndCommand.FlagClose
                            | ShapePath.FlagsAndCommand.FlagCW;

                    case StrokeMath.Status.Stop:
                        cmd = ShapePath.FlagsAndCommand.CommandStop;
                        break;
                }
            }
            return cmd;
        }
    }
}