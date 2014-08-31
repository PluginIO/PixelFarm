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
using System.Collections.Generic;

namespace MatterHackers.Agg.VertexSource
{
    // in the original agg this was conv_transform
    public class VertexSourceApplyTransform : IVertexSourceProxy
    {
        Transform.ITransform transformToApply;
        public VertexSourceApplyTransform(IVertexSource vertexSource, Transform.ITransform newTransformeToApply)
        {
            VertexSource = vertexSource;
            transformToApply = newTransformeToApply;
        }
        public IVertexSource VertexSource
        {
            get;
            set;
        }
        public bool IsDynamicVertexGen
        {
            get
            {
                return this.VertexSource.IsDynamicVertexGen;
            }

        }
        public IEnumerable<VertexData> GetVertexIter()
        {
            //transform 'on-the-fly' 
            foreach (VertexData vertexData in VertexSource.GetVertexIter())
            {
                
                VertexData transformedVertex = vertexData;
                if (ShapePath.IsVertextCommand(transformedVertex.command))
                {
                    //transform 2d
                    transformToApply.Transform(ref transformedVertex.position.x, ref transformedVertex.position.y);
                }
                
                yield return transformedVertex;
            }
        }

        public void Rewind(int path_id)
        {
            VertexSource.Rewind(path_id);
        }
        public void RewindZero()
        {
            VertexSource.RewindZero();
        }
        public ShapePath.FlagsAndCommand GetNextVertex(out double x, out double y)
        {
            ShapePath.FlagsAndCommand cmd = VertexSource.GetNextVertex(out x, out y);
            if (ShapePath.IsVertextCommand(cmd))
            {
                transformToApply.Transform(ref x, ref y);
            }
            return cmd;
        }

        public void SetTransformToApply(Transform.ITransform newTransformeToApply)
        {
            transformToApply = newTransformeToApply;
        }
    }
}