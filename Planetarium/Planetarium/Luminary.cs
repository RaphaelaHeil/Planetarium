using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Planetarium
{
    /// <summary>
    /// Represents a luminary, renders a sphere.
    /// </summary>
    class Luminary : SingleShapeContainer
    {       
        public String Name { get; set; }
        public String Info { get; set; }
        public Vector3 Coords { get; set; }  
      
        public float Scaling { get; set; }

        public float PrimaryDistance { get; set; }
        public float RotationSpeed { get; set; }
        public float Diameter { get; set; }

        public bool IsSatellite { get; set; }

        private int ExtraScale = 10;

        public List<Luminary> Satellites { get; set; }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="sphere">Shape that is rendered to represent this luminary</param>
        public Luminary(Shape sphere)
        {
            Shape = sphere;
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            Satellites = new List<Luminary>();
            PrimaryDistance = 0.0f;
        }

        /// <summary>
        /// Add a child luminary (satellite) to the list.
        /// </summary>
        /// <param name="child">child/satellite</param>
        public void add(Luminary child)
        {
            Satellites.Add(child);
        }

        /// <summary>
        /// Draw the luminary, i.e. prepare the modelview matrix and redirect to conatined shape
        /// </summary>
        /// <param name="time">Continuous time, used to render a constant movement</param>
        /// <param name="projection">Projection matrix</param>
        /// <param name="parentPosition">Parent's position</param>
        public void Draw(float time, Matrix4 projection, Vector3 parentPosition)
        {
            float x = (float)(Math.Cos((RotationSpeed * time) * Math.PI) * Coords.X) +parentPosition.X;
            float z = (float)(Math.Sin((RotationSpeed * time) * Math.PI) * Coords.Z) +parentPosition.Z; 
     
            Vector3 translation = new Vector3(x, 0, z);
            
            Model = Matrix4.CreateScale(Diameter * Scaling * ExtraScale ) * Matrix4.CreateRotationY(RotationSpeed * time) * Matrix4.CreateTranslation(translation);
            Projection = projection;
            BindTextures();
            Shape.Draw(Model, projection, Illuminated);
            foreach (var lum in Satellites)
            {
                lum.Draw(time, projection, translation);
            }
        }

        /// <summary>
        /// Initialise textures from the given file names (without base path/directory!).
        /// </summary>
        /// <param name="tex">regular texture filename</param>
        /// <param name="normal">normal map</param>
        /// <param name="bump">bump map</param>
        public void AddTextures(String tex, String normal, String bump)
        {
            Texture = new Bitmap(BasePath + @"\textures\" + tex);
            NormalMap = new Bitmap(BasePath + @"\textures\" + normal);
            BumpMap = new Bitmap(BasePath + @"\textures\" + bump);

            InitTextures();
        }

        /// <summary>
        /// Calculate center coordinates for sphere
        /// </summary>
        /// <param name="primaryPosition">One surface vector from the primary luminary ("center+radius")</param>
        /// <param name="primaryDistance">Distance to primary luminary surface</param>
        /// <param name="diameter">The shape's diameter</param>
        public void CalculateCoords(Vector3 primaryPosition, float primaryDistance, float diameter)
        {
            Diameter = diameter;

            if (primaryDistance == 0.0f)
            {
                PrimaryDistance = 0.0f;
                Coords = primaryPosition;
                return;
            }
            
            PrimaryDistance = (primaryDistance+0.5f*Diameter); // centerPrimary <--radius--> surfacePrimary <--distance--> surfaceLocal <--radius--> centerLocal
                      
            int r = 70;

            float x = (float)Math.Sqrt(r/100.0) * PrimaryDistance;
            float z = (float)Math.Sqrt((100 - r)/100.0) * PrimaryDistance;

            Coords = primaryPosition + new Vector3(x * Scaling * 0.01f, 0.0f, z * Scaling * 0.01f); // 
        }

        /// <summary>
        /// Calculate rotation speed for orbiting the sun
        /// </summary>
        /// <param name="speed"></param>
        public void CalculateSpeed(float speed)
        {
            if(speed!=0.0){
                 RotationSpeed = 1.0f / (speed*Scaling*0.001f);
            }else{
                RotationSpeed = 0.0f;
            }
        }
     }
}