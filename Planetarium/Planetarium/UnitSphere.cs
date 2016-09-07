using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Drawing.Imaging; 


namespace Planetarium
{

    /// <summary>
    /// Extends the Shape in the form of a unitsphere
    /// </summary>
    class UnitSphere : Shape
    {
       
        private int Rings = 70;

        private int Segments = 70;

     
        public UnitSphere(IdHolder holder)
        {
            Holder = holder; 
            GenerateVertices();
            PrepareAttributes();
            PrepareUniforms();
            PrepareBuffers();
        }

        public UnitSphere(IdHolder holder, int rings, int segments)
        {
            Rings = rings;
            Segments = segments;
            new UnitSphere(holder);
        }

        /// <summary>
        /// Draw the sphere with all needed fields filled/enabled.
        /// </summary>
        private void Draw()
        {
            FillBuffers();
            FillUniforms();
            EnableArrays();
      
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            DisableArrays();
            GL.Flush();
        }

        /// <summary>
        /// Draw the sphere using the given parameters.
        /// </summary>
        /// <param name="modelView">Modelview Matrix</param>
        /// <param name="projection">Projection Matrix</param>
        /// <param name="illuMode">Illumination mode</param>
        public override void Draw(Matrix4 modelView, Matrix4 projection, bool illuMode)
        {
            IlluMode = illuMode ? 1 : 0;
            ModelView = modelView;
            Projection = projection;
            this.Draw();
        }
          
        /// <summary>
        /// Generate Vertices for the unitsphere.
        /// Codesource: http://www.opentk.com/files/Geometries.cs (slightly modified)
        /// </summary>
        protected override void GenerateVertices()
        {
            double phiMax = 2 * Math.PI;
            double thetaMax = Math.PI;

            double phiIncrease = phiMax / Rings;
            double thetaIncrease = thetaMax / Segments;


            int vertexCount = Rings * Segments;

            Vertices = new Vector3[vertexCount];
            Normals = new Vector3[vertexCount]; //one normal for each vertex
            Texcoords = new Vector2[vertexCount];
            Indices = new int[vertexCount * 6]; //triangles -> 3 vertices per face, 2 faces per field/square
           
            int i = 0;

            for (double y = 0; y < Rings; y++)
            {
                double phi = (y / (Rings - 1) * 2 * Math.PI);
                for (double x = 0; x < Segments; x++)
                {
                    double theta = (x / (Segments - 1) * Math.PI);
                    Vector3 v = new Vector3(
                           (float)(Math.Sin(phi) * Math.Cos(theta)),
                           (float)(Math.Cos(phi)),
                           (float)(Math.Sin(phi) * Math.Sin(theta))
                       );
                    Vector3 n = Vector3.Normalize(v);
                    // Top - down texture projection.
                    Vector2 uv = new Vector2()
                    {
                        X = (float)(Math.Atan2(n.X, n.Z) / Math.PI / 2 + 0.5),
                        Y = (float)(Math.Asin(n.Y) / Math.PI + 0.5)
                    };
                   
                    Vertices[i] = v;
                    Normals[i] = n;
                    Texcoords[i] = uv;

                    i++;

                   
                }
            }
            //Calculate Indices:
            int j = 0;
            for (int y = 0; y < Rings - 1; y++)
            {
                for (int x = 0; x < Segments - 1; x++)
                {
                    Indices[j++] = ((y + 0) * Segments + x);
                    Indices[j++] = ((y + 1) * Segments + x);
                    Indices[j++] = ((y + 1) * Segments + x + 1);

                    Indices[j++] = ((y + 1) * Segments + x + 1);
                    Indices[j++] = ((y + 0) * Segments + x + 1);
                    Indices[j++] = ((y + 0) * Segments + x);
                }
            }
        }


       
    }
}





