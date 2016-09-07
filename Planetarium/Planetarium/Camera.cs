using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planetarium
{
   
    /// <summary>
    /// Interactive Camera Class, source: http://neokabuto.blogspot.de/2014/01/opentk-tutorial-5-basic-camera.html
    /// </summary>
    class Camera
    {

        private Vector3 Position = new Vector3(-150,150,0);
        private Vector3 Orientation = new Vector3(1.5f, 0f, 0f);
        private float Speed = 5.5f;
        private float MouseSensitivity = 0.01f;

        public float MovementSpeed { get { return Speed; } set { Speed = value; } }

        public Matrix4 GetViewMatrix()
        {
            Vector3 LookAt = new Vector3();

            LookAt.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            LookAt.Y = (float)Math.Sin((float)Orientation.Y);
            LookAt.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            return Matrix4.LookAt(Position, Position + LookAt, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 Offset = new Vector3();

            Vector3 Forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
            Vector3 Right = new Vector3(-Forward.Z, 0, Forward.X);

            Offset += x * Right;
            Offset += y * Forward;
            Offset.Y += z;

            Offset.NormalizeFast();
            Offset = Vector3.Multiply(Offset, Speed);

            Position += Offset;
        }

        public void Rotate(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);

        }




    }
}
