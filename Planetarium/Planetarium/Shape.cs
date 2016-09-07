using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planetarium
{
    /// <summary>
    /// Abstract super class for (untextured)shapes, containing all necessary fields to render the respective shape
    /// </summary>
    abstract class Shape
    {
        public Vector3[] Vertices { get; set; }

        public int[] Indices { get; set; }

        public Vector3[] Normals { get; set; }

        public Vector2[] Texcoords { get; set; }

        protected Matrix4 ModelView = Matrix4.Identity;

        protected Matrix4 Projection = Matrix4.Identity;

        protected int IlluMode = 0; //illumination mode (true = affected by point light, fals = not affected)

        public IdHolder Holder { get; set; }
       
        /// <summary>
        /// Prepare shader attributes/variables, i.e. retrieve ids for them using the shader identifiers specified in IdHolder ("vTexture", "vPosition", ...)
        /// </summary>
        protected void PrepareAttributes()
        {
            Holder.attribute_vpos = GL.GetAttribLocation(Holder.ProgId, IdHolder.position);
            Holder.attribute_vnormal = GL.GetAttribLocation(Holder.ProgId, IdHolder.normal);
            Holder.attribute_vtext = GL.GetAttribLocation(Holder.ProgId, IdHolder.texture);
        
            Console.Out.WriteLine(IdHolder.position + ": " + Holder.attribute_vpos);
            Console.Out.WriteLine(IdHolder.normal + ": " + Holder.attribute_vnormal);
            Console.Out.WriteLine(IdHolder.texture + ": " + Holder.attribute_vtext);

        }

        /// <summary>
        /// Prepare uniform shader variables, i.e. retrieve ids for them using the shader identifiers specified in IdHolder
        /// </summary>
        protected void PrepareUniforms()
        {
            Holder.uniformModelViewMatrix = GL.GetUniformLocation(Holder.ProgId, IdHolder.uni_model);
            Holder.uniformProjectionMatrix = GL.GetUniformLocation(Holder.ProgId, IdHolder.uni_projection);
            Holder.uniformShaderMode = GL.GetUniformLocation(Holder.ProgId, IdHolder.mode);
            Holder.uniformIlluminationMode = GL.GetUniformLocation(Holder.ProgId, IdHolder.illuMode);


            Console.Out.WriteLine(IdHolder.uni_model + ": " + Holder.uniformModelViewMatrix);
            Console.Out.WriteLine(IdHolder.uni_projection + ": " + Holder.uniformProjectionMatrix);
            Console.Out.WriteLine(IdHolder.mode + ":" + Holder.uniformShaderMode);
        }

        /// <summary>
        /// Prepare buffers, i.e. retrieve ids by generating buffers
        /// </summary>
        protected void PrepareBuffers()
        {
            int pos,  norm, el, tex;
            GL.GenBuffers(1, out pos);
            GL.GenBuffers(1, out norm);
            GL.GenBuffers(1, out tex);
            GL.GenBuffers(1, out el);

            Holder.SetBufferIds(pos,  norm, el, tex);

        }

        /// <summary>
        /// Enable the VertexArrays before rendering the respective shape
        /// </summary>
        protected void EnableArrays()
        {
            GL.EnableVertexAttribArray(Holder.attribute_vpos);
            GL.EnableVertexAttribArray(Holder.attribute_vnormal);
            GL.EnableVertexAttribArray(Holder.attribute_vtext);
        }

        /// <summary>
        /// Disable the VertexArrays after rendering the respective shape
        /// </summary>
        protected void DisableArrays()
        {
            GL.DisableVertexAttribArray(Holder.attribute_vpos);
            GL.DisableVertexAttribArray(Holder.attribute_vnormal);
            GL.DisableVertexAttribArray(Holder.attribute_vtext);
        }

        /// <summary>
        /// Fill the Buffer Objects with the previously generated values.
        /// </summary>
        protected void FillBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Holder.vbo_position); //bind buffer that will be modified
            //load data into selected buffer
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * Vector3.SizeInBytes), Vertices, BufferUsageHint.StaticDraw);
            //assign shader attribute/variable to hold single buffer entries during rendering
            GL.VertexAttribPointer(Holder.attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Holder.vbo_normal);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Normals.Length * Vector3.SizeInBytes), Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Holder.attribute_vnormal, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Holder.vbo_text);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Texcoords.Length * Vector2.SizeInBytes), Texcoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Holder.attribute_vtext, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Holder.ibo_elements); //for indices: bind elementarraybuffer instead
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(int)), Indices, BufferUsageHint.StaticDraw);
            //no need to assign an attributepointer since this only represents the indices needed to select entries from the buffers above

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //bind empty id to make sure no following calls modify any of the above buffers accidentally 
        }

        /// <summary>
        /// Fill the shader uniforms with the given values.
        /// </summary>
        protected void FillUniforms()
        {
            GL.UniformMatrix4(Holder.uniformModelViewMatrix, false, ref ModelView);
            GL.UniformMatrix4(Holder.uniformProjectionMatrix, false, ref Projection);
            GL.Uniform1(Holder.uniformShaderMode, Holder.ShaderMode);           
            GL.Uniform1(Holder.uniformIlluminationMode, IlluMode);
            
        }
        
        /// <summary>
        /// Generate Vertices, implementation depends on the type of subclass (e.g. sphere vs. cube, ...)
        /// </summary>
        protected abstract void GenerateVertices();
        
        /// <summary>
        /// Implementation depends on the type of subclass, e.g. Triangles vs Quads, etc.
        /// </summary>
        /// <param name="modelView">ModelView Matrix</param>
        /// <param name="projection">Projection Matrix</param>
        /// <param name="illuMode">Illumination Mode -> affected by the sun/light source or not</param>
        public abstract void Draw(Matrix4 modelView, Matrix4 projection, bool illuMode);

    }
}
