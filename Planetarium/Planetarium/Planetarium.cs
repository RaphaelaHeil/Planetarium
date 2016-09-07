using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace Planetarium
{
    //textures: http://www.supinemusic.net/Files%20for%20Cloud%20projects/Cinema%204D/plugins/Planet%20X%20Generator%20R12/presets/
    //http://neokabuto.blogspot.de/2013/03/opentk-tutorial-2-drawing-triangle.html
    class Planetarium : GameWindow
    {

        private IdHolder holder;

        private Camera cam;

        private UnitSphere sphere;

        private float time = 0.0f;

        private Vector2 lastMousePos = new Vector2();

        private String BasePath;

        private List<Luminary> lums;

        private bool rotating = true;


        protected override void OnLoad(EventArgs e)
        {
            BasePath = AppDomain.CurrentDomain.BaseDirectory;

            base.OnLoad(e);
            Title = "Planetarium";

            holder = new IdHolder();
            holder.Height = Height;
            holder.Width = Width;
            holder.ShaderMode = 1;
            InitProgram();
            sphere = new UnitSphere(holder);

            lums = LuminaryCreator.Create(BasePath + @"\universe.xml", sphere); //load luminaries from xml

            cam = new Camera();

            GL.ClearColor(Color.CornflowerBlue);       
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 projection = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 0.01f, 10000000.0f);
         
            foreach (var child in lums)
            {
                child.Draw(time, projection, new Vector3(0,0,0));
            }
           
            SwapBuffers();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

           
            if (e.KeyChar == 'b') //exit window
            {
                Exit();
            }
            if (e.KeyChar == 49)
            {
                holder.ShaderMode = 1; //regular textures
            }
            if (e.KeyChar == 50)
            {
                holder.ShaderMode = 2; //normal maps
            }
            if (e.KeyChar == 51)
            {
                holder.ShaderMode = 3; //bump maps
            }
            if (e.KeyChar == 52)
            {
                holder.ShaderMode = 4; //mini "cel" shader
            }
            if (e.KeyChar == 'p') //pause rotation
            {
                rotating = !rotating;               
                if (rotating)
                {
                    cam.MovementSpeed = 5.5f;
                }
                else
                {
                    cam.MovementSpeed = 30.0f;
                }
            }

            switch (e.KeyChar)
            {
                case 'w':
                    cam.Move(0f, 0.1f, 0f); //move in +y direction
                    break;
                case 'a':
                    cam.Move(-0.1f, 0f, 0f); //move in -x direction
                    break;
                case 's':
                    cam.Move(0f, -0.1f, 0f); //move in -y direction
                    break;
                case 'd':
                    cam.Move(0.1f, 0f, 0f); //move in +x direction
                    break;
                case 'q':
                    cam.Move(0f, 0f, 0.1f); //move in +z direction
                    break;
                case 'e':
                    cam.Move(0f, 0f, -0.1f); //move in -z direction
                    break;
            }

           
        }

        void ResetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (rotating)
            {
                time += (float)e.Time;
            }

            if (Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

                cam.Rotate(delta.X, delta.Y);
                ResetCursor();
            }
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (Focused)
            {
                ResetCursor();
            }
        }

        private void InitProgram()
        {
            int vert, frag;

            holder.ProgId = GL.CreateProgram();
            LoadShader("vertex.glsl", ShaderType.VertexShader, holder.ProgId, out vert);
            LoadShader("fragment.glsl", ShaderType.FragmentShader, holder.ProgId, out frag);

            holder.VertexId = vert;
            holder.FragmentId = frag;

            GL.LinkProgram(holder.ProgId);
      
            GL.UseProgram(holder.ProgId);

            GL.DetachShader(holder.ProgId, holder.VertexId);
            GL.DetachShader(holder.ProgId, holder.FragmentId);
           
        }

        private void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
    }
}
