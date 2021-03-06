﻿//MIT, 2014-2016,WinterDev
//creadit : http://learningwebgl.com/lessons/lesson16/index.html

using System;
using Mini;
using PixelFarm.DrawingGL;
using OpenTK.Graphics.ES20;

namespace OpenTkEssTest
{
    [Info(OrderCode = "115")]
    [Info("T115_VBO_01")]
    public class T115_VBO_01 : DemoBase
    {
        CanvasGL2d canvas2d;
        GLCanvasPainter painter;
        PixelFarm.Drawing.RenderVx polygon1, polygon2, polygon3;
        bool isInit;
        bool frameBufferNeedUpdate;
        protected override void OnGLContextReady(CanvasGL2d canvasGL, GLCanvasPainter painter)
        {
            this.canvas2d = canvasGL;
            this.painter = painter;
        }
        protected override void OnReadyForInitGLShaderProgram()
        {
            frameBufferNeedUpdate = true;
            polygon1 = painter.CreatePolygonRenderVx(new float[]
            {
                50,200,
                250,200,
                125,350
            });
            polygon2 = painter.CreatePolygonRenderVx(new float[]
             {
                   200, 50,
                   250, 50,
                   210, 100
             });
            polygon3 = painter.CreatePolygonRenderVx(new float[]
              {
                 400, 50,
                 450, 50,
                 410, 100,
                 350, 100,
                 200,50,
                 100,20
              });
        }
        protected override void DemoClosing()
        {
            canvas2d.Dispose();
        }
        protected override void OnGLRender(object sender, EventArgs args)
        {

            canvas2d.SmoothMode = CanvasSmoothMode.Smooth;
            canvas2d.StrokeColor = PixelFarm.Drawing.Color.Blue;
            canvas2d.Clear(PixelFarm.Drawing.Color.White);
            canvas2d.ClearColorBuffer();
            //-------------------------------
            if (!isInit)
            {
                isInit = true;
            }
            canvas2d.Clear(PixelFarm.Drawing.Color.Blue);
            painter.StrokeColor = PixelFarm.Drawing.Color.Black;
            painter.StrokeWidth = 2;
            //-------------------------------
            //painter.FillColor = PixelFarm.Drawing.Color.Yellow;
            //painter.FillRenderVx(polygon1);
            //-------------------------------
            //painter.FillColor = PixelFarm.Drawing.Color.Red;
            //painter.FillRenderVx(polygon2);
            //////-------------------------------
            painter.FillColor = PixelFarm.Drawing.Color.Magenta;
            try
            {
                painter.FillRenderVx(polygon3);
                SwapBuffers();
            }
            catch (Exception ex)
            {
            }
        }
    }
}

