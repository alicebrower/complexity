﻿using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complexity.Objects;
using Complexity.Util;
using Complexity.Managers;

namespace Complexity.Main {
    /// <summary>
    /// Creates a window for drawing and handles all things related to that.
    /// All data and logic are stored in the universe and it's constiuents, this is just the GUI window.
    /// </summary>
    public class RenderWindow : GameWindow {
        private Matrix4 matrixProjection, matrixModelview;
        public float cameraRotation = 0f;
        private Scene renderScene;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public RenderWindow(Scene scene)
            : base(800, 600, new GraphicsMode(32, 24, 0, 8), "Complexity") {
                renderScene = scene;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e) {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(EnableCap.VertexArray);
            GL.EnableClientState(EnableCap.ColorArray);

            //Setup input things
            Keyboard.KeyDown += InputManager.HandleKeyDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            matrixProjection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1f, 100f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrixProjection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e) {
            if (Keyboard[Key.Escape])
                Exit();

            //Camera Controls
            if(Keyboard[Key.Q])
                cameraRotation = (cameraRotation + 0.1f) % 360;
            if (Keyboard[Key.E])
                cameraRotation = (cameraRotation - 0.1f) % 360;

            base.OnUpdateFrame(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4.CreateRotationY(cameraRotation, out matrixModelview);
            matrixModelview *= Matrix4.LookAt(0f, 0f, -5f, 0f, 0f, 0f, 0f, 1f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matrixModelview);

            //Recalculate
            //This should be modified so that everything is calculated to a buffer
            //That could be done on a separate thread, then, when that's over
            //we can swap buffers after rendering
            //ExpressionManager.Recalculate();
            ExecutionManager.Recalculate();

            //Draw the scene
            renderScene.Draw();

            SwapBuffers();
        }
    }
}
