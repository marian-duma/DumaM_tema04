using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramMain
{
    internal class Window : GameWindow
    {
        Color DEFAULT_BKG_COLOR = Color.LightBlue;
        public Window() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(DEFAULT_BKG_COLOR);
            //Obiectele apropiate le ascund pe cele departate
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.DontCare);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            
            //Setare perspectiva
            Matrix4 persperctive = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 256);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref persperctive);

            //Setare camera
            Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }
        
        //Metoda care se ocupa cu logica aplicatiei.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            base.OnUpdateFrame(e);
        }

        //Metoda care se ocupa cu randararea scenei.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();
            //GL.Rotate(-MathHelper.PiOver4, 0, 0, 1);
            GL.Color3(Color.Black);

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, -50);
            GL.Color3(Color.Green);
            GL.Vertex3(10, 5, -50);
            GL.Color3(Color.Blue);
            GL.Vertex3(5, 10, -50);

            GL.End();
            GL.PopMatrix();

            //Randare
            SwapBuffers();
        }
    }
}
