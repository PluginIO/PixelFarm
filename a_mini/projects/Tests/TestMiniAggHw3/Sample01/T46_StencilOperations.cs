﻿//
// Copyright (c) 2014 The ANGLE Project Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.
//

//            Based on Simple_VertexShader.c from
// Book:      OpenGL(R) ES 2.0 Programming Guide
// Authors:   Aaftab Munshi, Dan Ginsburg, Dave Shreiner
// ISBN-10:   0321502795
// ISBN-13:   9780321502797
// Publisher: Addison-Wesley Professional
// URLs:      http://safari.informit.com/9780321563835
//            http://www.opengles-book.com



#region Using Directives

using System;
using OpenTK.Graphics.ES20;
using Mini;
#endregion

namespace OpenTkEssTest
{
    [Info(OrderCode = "046")]
    [Info("T46_StencilOperations")]
    public class T46_StencilOperations : PrebuiltGLControlDemoBase
    {
        bool isGLInit;
        protected override void OnInitGLProgram(object sender, EventArgs args)
        {
            string vs = @"
                 attribute vec4 a_position;
                 void main()
                 {
                         gl_Position = a_position;
                 }
            ";
            string fs = @"
                 precision mediump float;   
                 uniform vec4 u_color;
                 void main()
                 {
                        gl_FragColor = u_color;
                 }
            ";
            mProgram = ES2Utils.CompileProgram(vs, fs);
            if (mProgram == 0)
            {
                throw new NotSupportedException();
            }

            //// Get the attribute locations
            //mPositionLoc = glGetAttribLocation(mProgram, "a_position");
            mPositionLoc = GL.GetAttribLocation(mProgram, "a_position");
            //// Get the sampler location
            //mColorLoc = glGetUniformLocation(mProgram, "u_color");
            mColorLoc = GL.GetUniformLocation(mProgram, "u_color");
            //// Set the clear color
            //glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.ClearColor(0, 0, 0, 0);
            //// Set the stencil clear value
            //glClearStencil(0x01);
            GL.ClearStencil(0x01);
            //// Set the depth clear value
            //glClearDepthf(0.75f);
            GL.ClearDepth(0.75f);
            //// Enable the depth and stencil tests
            //glEnable(GL_DEPTH_TEST);
            GL.Enable(EnableCap.DepthTest);
            //glEnable(GL_STENCIL_TEST);
            GL.Enable(EnableCap.StencilTest);
            //return true;

            isGLInit = true;
            this.EnableAnimationTimer = true;
        }
        protected override void OnGLRender(object sender, EventArgs args)
        {
            if (!isGLInit)
            {
                return;
            }
            //GLfloat vertices[] =
            //      {
            //          -0.75f,  0.25f, 0.50f, // Quad #0
            //          -0.25f,  0.25f, 0.50f,
            //          -0.25f,  0.75f, 0.50f,
            //          -0.75f,  0.75f, 0.50f,
            //           0.25f,  0.25f, 0.90f, // Quad #1
            //           0.75f,  0.25f, 0.90f,
            //           0.75f,  0.75f, 0.90f,
            //           0.25f,  0.75f, 0.90f,
            //          -0.75f, -0.75f, 0.50f, // Quad #2
            //          -0.25f, -0.75f, 0.50f,
            //          -0.25f, -0.25f, 0.50f,
            //          -0.75f, -0.25f, 0.50f,
            //           0.25f, -0.75f, 0.50f, // Quad #3
            //           0.75f, -0.75f, 0.50f,
            //           0.75f, -0.25f, 0.50f,
            //           0.25f, -0.25f, 0.50f,
            //          -1.00f, -1.00f, 0.00f, // Big Quad
            //           1.00f, -1.00f, 0.00f,
            //           1.00f,  1.00f, 0.00f,
            //          -1.00f,  1.00f, 0.00f,
            //      };
            float[] vertices = new float[]{
                 -0.75f,  0.25f, 0.50f, // Quad #0
                -0.25f,  0.25f, 0.50f,
                -0.25f,  0.75f, 0.50f,
                -0.75f,  0.75f, 0.50f,
                 0.25f,  0.25f, 0.90f, // Quad #1
                 0.75f,  0.25f, 0.90f,
                 0.75f,  0.75f, 0.90f,
                 0.25f,  0.75f, 0.90f,
                -0.75f, -0.75f, 0.50f, // Quad #2
                -0.25f, -0.75f, 0.50f,
                -0.25f, -0.25f, 0.50f,
                -0.75f, -0.25f, 0.50f,
                 0.25f, -0.75f, 0.50f, // Quad #3
                 0.75f, -0.75f, 0.50f,
                 0.75f, -0.25f, 0.50f,
                 0.25f, -0.25f, 0.50f,
                -1.00f, -1.00f, 0.00f, // Big Quad
                 1.00f, -1.00f, 0.00f,
                 1.00f,  1.00f, 0.00f,
                -1.00f,  1.00f, 0.00f,
           };
            //      GLubyte indices[][6] =
            //      {
            //          {  0,  1,  2,  0,  2,  3 }, // Quad #0
            //          {  4,  5,  6,  4,  6,  7 }, // Quad #1
            //          {  8,  9, 10,  8, 10, 11 }, // Quad #2
            //          { 12, 13, 14, 12, 14, 15 }, // Quad #3
            //          { 16, 17, 18, 16, 18, 19 }, // Big Quad
            //      };

            byte[][] indices = new byte[][]{
               new byte[]{  0,  1,  2,  0,  2,  3 }, // Quad #0
               new byte[]{  4,  5,  6,  4,  6,  7 }, // Quad #1
               new byte[]{  8,  9, 10,  8, 10, 11 }, // Quad #2
               new byte[]{ 12, 13, 14, 12, 14, 15 }, // Quad #3
               new byte[]{ 16, 17, 18, 16, 18, 19 }, // Big Quad
            };
            //      static const size_t testCount = 4;
            int testCount = 4;
            //      GLfloat colors[testCount][4] =
            //      {
            //          { 1.0f, 0.0f, 0.0f, 1.0f },
            //          { 0.0f, 1.0f, 0.0f, 1.0f },
            //          { 0.0f, 0.0f, 1.0f, 1.0f },
            //          { 1.0f, 1.0f, 0.0f, 0.0f },
            //      };
            float[][] colors = new float[][]{
                new float[]{ 1.0f, 0.0f, 0.0f, 1.0f },
                new float[]{ 0.0f, 1.0f, 0.0f, 1.0f },
                new float[]{ 0.0f, 0.0f, 1.0f, 1.0f },
                new float[]{ 1.0f, 1.0f, 0.0f, 0.0f },
            };
            //      GLuint stencilValues[testCount] =
            //      {
            //          0x7, // Result of test 0
            //          0x0, // Result of test 1
            //          0x2, // Result of test 2
            //          0xff // Result of test 3.  We need to fill this value in at run-time
            //      };
            int[] stencilValues = new int[]{
                0x7, // Result of test 0
                0x0, // Result of test 1
                0x2, // Result of test 2
                0xff // Result of test 3.  We need to fill this value in at run-time
            };
            //      // Set the viewport
            //      glViewport(0, 0, getWindow()->getWidth(), getWindow()->getHeight());
            GL.Viewport(0, 0, this.Width, this.Height);
            //      // Clear the color, depth, and stencil buffers.  At this point, the stencil
            //      // buffer will be 0x1 for all pixels
            //      glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            //      // Use the program object
            //      glUseProgram(mProgram);
            GL.UseProgram(mProgram);
            //      // Load the vertex data
            //      glVertexAttribPointer(mPositionLoc, 3, GL_FLOAT, GL_FALSE, 0, vertices);
            GL.VertexAttribPointer(mPositionLoc, 3, VertexAttribPointerType.Float, false, 0, vertices);
            //      glEnableVertexAttribArray(mPositionLoc);
            GL.EnableVertexAttribArray(mPositionLoc);
            //      // Test 0:
            //      //
            //      // Initialize upper-left region.  In this case, the stencil-buffer values will
            //      // be replaced because the stencil test for the rendered pixels will fail the
            //      // stencil test, which is
            //      //
            //      //      ref   mask   stencil  mask
            //      //    ( 0x7 & 0x3 ) < ( 0x1 & 0x7 )
            //      //
            //      // The value in the stencil buffer for these pixels will be 0x7.
            //      glStencilFunc(GL_LESS, 0x7, 0x3);
            GL.StencilFunc(StencilFunction.Lequal, 0x7, 0x3);
            //      glStencilOp(GL_REPLACE, GL_DECR, GL_DECR);
            GL.StencilOp(StencilOp.Replace, StencilOp.Decr, StencilOp.Decr);
            //      glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_BYTE, indices[0]);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices[0]);
            //      // Test 1:
            //      //
            //      // Initialize the upper-right region. Here, we'll decrement the stencil-buffer
            //      // values where the stencil test passes but the depth test fails. The stencil test is
            //      //
            //      //      ref  mask    stencil  mask
            //      //    ( 0x3 & 0x3 ) > ( 0x1 & 0x3 )
            //      //
            //      // but where the geometry fails the depth test. The stencil values for these pixels
            //      // will be 0x0.
            //      glStencilFunc(GL_GREATER, 0x3, 0x3);
            //      glStencilOp(GL_KEEP, GL_DECR, GL_KEEP);
            //      glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_BYTE, indices[1]);


            GL.StencilFunc(StencilFunction.Greater, 0x3, 0x3);
            GL.StencilOp(StencilOp.Keep, StencilOp.Decr, StencilOp.Keep);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices[1]);
            //      // Test 2:
            //      //
            //      // Initialize the lower-left region.  Here we'll increment (with saturation) the
            //      // stencil value where both the stencil and depth tests pass.  The stencil test for
            //      // these pixels will be
            //      //
            //      //      ref  mask     stencil  mask
            //      //    ( 0x1 & 0x3 ) == ( 0x1 & 0x3 )
            //      //
            //      // The stencil values for these pixels will be 0x2.
            //      glStencilFunc(GL_EQUAL, 0x1, 0x3);
            //      glStencilOp(GL_KEEP, GL_INCR, GL_INCR);
            //      glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_BYTE, indices[2]);
            GL.StencilFunc(StencilFunction.Equal, 0x1, 0x3);
            GL.StencilOp(StencilOp.Keep, StencilOp.Incr, StencilOp.Incr);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices[2]);
            //      // Test 3:
            //      //
            //      // Finally, initialize the lower-right region.  We'll invert the stencil value
            //      // where the stencil tests fails. The stencil test for these pixels will be
            //      //
            //      //      ref   mask    stencil  mask
            //      //    ( 0x2 & 0x1 ) == ( 0x1 & 0x1 )
            //      //
            //      // The stencil value here will be set to ~((2^s-1) & 0x1), (with the 0x1 being
            //      // from the stencil clear value), where 's' is the number of bits in the stencil
            //      // buffer
            //      glStencilFunc(GL_EQUAL, 0x2, 0x1);
            //      glStencilOp(GL_INVERT, GL_KEEP, GL_KEEP);
            //      glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_BYTE, indices[3]);
            GL.StencilFunc(StencilFunction.Equal, 0x2, 0x1);
            GL.StencilOp(StencilOp.Invert, StencilOp.Keep, StencilOp.Keep);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices[3]);
            //      // Since we don't know at compile time how many stencil bits are present, we'll
            //      // query, and update the value correct value in the  stencilValues arrays for the
            //      // fourth tests. We'll use this value later in rendering.
            //      GLint stencilBitCount;
            //      glGetIntegerv(GL_STENCIL_BITS, &stencilBitCount);
            //      stencilValues[3] = ~(((1 << stencilBitCount) - 1) & 0x1) & 0xff;
            int stencilBitCount;
            GL.GetInteger(GetPName.StencilBits, out stencilBitCount);
            stencilValues[3] = ~(((1 << stencilBitCount) - 1) & 0x1) & 0xff;
            //      // Use the stencil buffer for controlling where rendering will occur.  We disable
            //      // writing to the stencil buffer so we can test against them without modifying
            //      // the values we generated.
            //      glStencilMask(0x0);
            GL.StencilMask(0x0);
            //      for (size_t i = 0; i < testCount; ++i)
            //      {
            //          glStencilFunc(GL_EQUAL, stencilValues[i], 0xff);
            //          glUniform4fv(mColorLoc, 1, colors[i]);
            //          glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_BYTE, indices[4]);
            //      }
            for (int i = 0; i < testCount; ++i)
            {
                GL.StencilFunc(StencilFunction.Equal, stencilValues[i], 0xff);
                GL.Uniform4(mColorLoc, 1, colors[i]);
                GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices[4]);
                GL.StencilFunc(StencilFunction.Equal, stencilValues[i], 0xff);
            }
            //      // Reset the stencil mask
            //      glStencilMask(0xFF);
            GL.StencilMask(0xFF);
            SwapBuffers();
        }
        protected override void DemoClosing()
        {
            GL.DeleteProgram(mProgram);
            mProgram = 0;
        }
        // Handle to a program object
        int mProgram;
        // Attribute locations
        int mPositionLoc;
        // Uniform locations
        int mColorLoc;
    }
}