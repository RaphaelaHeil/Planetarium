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
    /// Abstract super class for classes that are rendered using a Shape. 
    /// </summary>
    abstract class SingleShapeContainer
    {
        protected Shape Shape;
        protected Bitmap Texture;
        protected Bitmap NormalMap;
        protected Bitmap BumpMap;
        public bool Illuminated { get; set; } //affected by the point light, needed to differ between the sun&skybox and all other objects

        private int uniformTexture;
        private int uniformNormalMap;
        private int uniformBumpMap;

        protected Matrix4 Model;
        protected Matrix4 Projection;

        protected String BasePath;

        /// <summary>
        /// Init all textures, i.e. regular texture, normal & bump map.
        /// </summary>
        public void InitTextures()
        {
            CreateTexture(out uniformTexture, Texture); 
            CreateTexture(out uniformNormalMap, NormalMap);
            CreateTexture(out uniformBumpMap, BumpMap);
        }

        /// <summary>
        /// Bind all textures, i.e. regular texture, normal & bump map.
        /// </summary>
        public void BindTextures()
        {
            BindTexture(uniformTexture, TextureUnit.Texture1, IdHolder.uni_texture);
            BindTexture(uniformNormalMap, TextureUnit.Texture2, IdHolder.uni_normal);
            BindTexture(uniformBumpMap, TextureUnit.Texture3, IdHolder.uni_bump);
        }

        /// <summary>
        /// Generate a new texture from the given BitMap and bind it to the id that is returned.
        /// </summary>
        /// <param name="textureId">id of bound texture</param>
        /// <param name="map">Bitmap to bind</param>
        private void CreateTexture(out int textureId, Bitmap map)
        {
            GL.GenTextures(1, out textureId); //allocate texture id
            GL.BindTexture(TextureTarget.Texture2D, textureId); //bind id
         
            BitmapData data = map.LockBits(new System.Drawing.Rectangle(0, 0, map.Width, map.Height),
             ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //load data 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
            OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            //release lock
            map.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        }

        /// <summary>
        /// Bind the texture before rendering the respective object.
        /// </summary>
        /// <param name="textureId">Texture id </param>
        /// <param name="unit">current texture unit</param>
        /// <param name="uniformName">unform shader identifier (e.g. "bump")</param>
        private void BindTexture(int textureId, TextureUnit unit, string uniformName)
        {
            GL.ActiveTexture(unit); //select active texture
            GL.BindTexture(TextureTarget.Texture2D, textureId); //bind texture
            GL.Uniform1(GL.GetUniformLocation(Shape.Holder.ProgId, uniformName), unit - TextureUnit.Texture0); //assign texture to uniform shader identifier
        }
    }
}
