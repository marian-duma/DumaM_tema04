using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace ProgramMain
{
    internal class Window : GameWindow
    {
        Color DEFAULT_BKG_COLOR = Color.LightBlue;
        Camera3DIsometric camera;
        Axes axes;
        Grid grid;
        MouseState prevMouse;
        KeyboardState prevInput;
        private List<Objectoid> objects;
        //Verifica gravitatia pentru toate obiectele
        //Astfel nu vor fi obiecte cu gravitatia activata si altele nu.
        private bool gravityEnabled;
        public Window() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;
            camera = new Camera3DIsometric(120, 30, 80, 0, 0, 0, 0, 1, 0);
            axes = new Axes();
            grid = new Grid();
            objects = new List<Objectoid>();
            DisplayHelp();
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
            Matrix4 persperctive = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 250);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref persperctive);

            //Setare camera
            camera.SetCamera();
        }
        
        //Metoda care se ocupa cu logica aplicatiei.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if(input.IsKeyDown(Key.D))
            {
                camera.MoveRight();
            }
            if (input.IsKeyDown(Key.A))
            {
                camera.MoveLeft();
            }
            if (input.IsKeyDown(Key.W))
            {
                camera.MoveForward();
            }
            if (input.IsKeyDown(Key.S))
            {
                camera.MoveBackward();
            }
            if (input.IsKeyDown(Key.Q))
            {
                camera.MoveUp();
            }
            if (input.IsKeyDown(Key.E))
            {
                camera.MoveDown();
            }
            if (input.IsKeyDown(Key.K) && !prevInput.IsKeyDown(Key.K))
            {
                axes.ToggleVisibility();
            }
            if (input.IsKeyDown(Key.V) && !prevInput.IsKeyDown(Key.V))
            {
                grid.ToggleVisibility();
            }
            if (input.IsKeyDown(Key.R) && !prevInput.IsKeyDown(Key.R))
            {
                //Reseteaza camera la valori initiale
                camera = new Camera3DIsometric(120, 30, 80, 0, 0, 0, 0, 1, 0);
                camera.SetCamera();

                //Elimina toate obiectele
                objects.Clear();
                
                GL.ClearColor(DEFAULT_BKG_COLOR);
            }
            if (input.IsKeyDown(Key.B) && !prevInput.IsKeyDown(Key.B))
            {
                Randomizer rando = new Randomizer();
                Color randomColor = rando.GetRandomColor();
                GL.ClearColor(randomColor);
            }
            if (input.IsKeyDown(Key.G) && !prevInput.IsKeyDown(Key.G))
            {
                gravityEnabled = !gravityEnabled;
                foreach (Objectoid obj in objects)
                {
                    obj.setGravity(gravityEnabled);
                }
            }
            if(input.IsKeyDown(Key.H) && !prevInput.IsKeyDown(Key.H))
            {
                DisplayHelp();
            }
            if (mouse[MouseButton.Left] && !prevMouse[MouseButton.Left])
            {
                Objectoid obj = new Objectoid();
                obj.setGravity(gravityEnabled);
                objects.Add(obj);
            }
            if (mouse[MouseButton.Right] && !prevMouse[MouseButton.Right])
            {
                objects.Clear();
            }
            prevInput = input;
            prevMouse = mouse;

            //Falling logic
            foreach (Objectoid obj in objects)
            {
                obj.UpdatePosition();
            }

            //Not true rotation
            //if (input.IsKeyDown(Key.Up))
            //{
            //    camera.RotateUp();
            //}
            //if (input.IsKeyDown(Key.Down))
            //{
            //    camera.RotateDown();
            //}
            //if (input.IsKeyDown(Key.Left))
            //{
            //    camera.RotateLeft();
            //}
            //if (input.IsKeyDown(Key.Right))
            //{
            //    camera.RotateRight();
            //}
        }

        //Metoda care se ocupa cu randararea scenei.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            axes.Draw();
            grid.Draw();
            foreach (Objectoid obj in objects)
            {
                obj.Draw();
            }

            //Randare
            SwapBuffers();
        }
        public void DisplayHelp()
        {
            Console.WriteLine("\n       Meniu");
            Console.WriteLine("-----------------------");
            Console.WriteLine("H - Meniu");
            Console.WriteLine("K - Schimbare vizibilitate sistem de axe");
            Console.WriteLine("R - Reseteaza scena la valori implicite");
            Console.WriteLine("B - Schimbare culoare de fundal");
            Console.WriteLine("V - Schimbare vizibilitate grid");
            Console.WriteLine("G - Manipuleaza gravitatia");
            Console.WriteLine("Click stanga mouse - Adauga obiect");
            Console.WriteLine("Click drepta mouse - Sterge obiecte");
            Console.WriteLine("ESC - Parasire aplicatie");
            Console.WriteLine("W, A, S, D, Q, E - Miscare camera");

        }
    }
}
