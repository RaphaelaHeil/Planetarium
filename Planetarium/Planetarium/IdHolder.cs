using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planetarium
{
    /// <summary>
    /// Helper class that contains all shader ids and string-identifiers that are the same for everything that is rendered.
    /// Not included: texture ids since these are assigned and held by "Luminary". 
    /// </summary>
    class IdHolder
    {
        /// <summary>
        /// Vertex position id
        /// </summary>
        public int attribute_vpos { get; set; }

        /// <summary>
        /// Shader identifier for vertex position ("vPosition")
        /// </summary>
        public static String position = "vPosition";

        /// <summary>
        /// Texture position id
        /// </summary>
        public int attribute_vtext { get; set; }
        
        /// <summary>
        /// Shader identifier for texture position ("vTexture")
        /// </summary>
        public static String texture = "vTexture";
        
        /// <summary>
        /// Normal position id
        /// </summary>
        public int attribute_vnormal { get; set; }

        /// <summary>
        /// Shader identifier for normal position ("vNormal")
        /// </summary>
        public static String normal = "vNormal";

        /// <summary>
        /// Vertex Buffer Object id for positions
        /// </summary>
        public int vbo_position { get; set; }

        /// <summary>
        /// Vertex Buffer Object id for normals
        /// </summary>
        public int vbo_normal { get; set; }

        /// <summary>
        /// Vertex Buffer Object id for texture-coords
        /// </summary>
        public int vbo_text { get; set; }

        /// <summary>
        /// Index Buffer Object id
        /// </summary>
        public int ibo_elements { get; set; }
        
        /// <summary>
        /// Id for uniform ModelViewMatrix
        /// </summary>
        public int uniformModelViewMatrix { get; set; }

        /// <summary>
        /// Sahder identifier for uniform ModelViewMatrix ("mModel")
        /// </summary>
        public static String uni_model = "mModel";

        /// <summary>
        /// Id for uniform ProjectionMatrix
        /// </summary>
        public int uniformProjectionMatrix { get; set; }

        /// <summary>
        /// Shader identifier for uniform projection matrix ("mProjection")
        /// </summary>
        public static String uni_projection = "mProjection";

        /// <summary>
        /// Shader identifier for uniform texture ("texture")
        /// </summary>
        public const String uni_texture = "texture";

        /// <summary>
        /// Shader identifier for uniform bump map ("bump")
        /// </summary>
        public const String uni_bump = "bump";

        /// <summary>
        /// Shader identifier for uniform nomal map ("mNormal")
        /// </summary>
        public const String uni_normal = "mNormal";

        /// <summary>
        /// Program id
        /// </summary>
        public int ProgId { get; set; }

        /// <summary>
        /// Vertex Shader id
        /// </summary>
        public int VertexId { get; set; }

        /// <summary>
        /// Fragment Shader id
        /// </summary>
        public int FragmentId { get; set; }

        /// <summary>
        /// Game Window Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Game Window Height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// ShaderMode, to switch between different FragmentShader methods (this is the actual value, for the id see <paramref name="uniformShaderMode"/>)
        /// </summary>
        public int ShaderMode { get; set; }
        
        /// <summary>
        /// Id for uniform ShaderMode
        /// </summary>
        public int uniformShaderMode { get; set; }

        /// <summary>
        /// Shader identifier for uniform ShaderMode ("mode")
        /// </summary>
        public static String mode = "mode";

        /// <summary>
        /// Id for uniform IlluminationMode
        /// </summary>
        public int uniformIlluminationMode { get; set; }

        /// <summary>
        /// Shader identifier for uniform IlluminationMode ("illu")
        /// </summary>
        public static String illuMode = "illu";

        /// <summary>
        /// Set all position, normal, texture and index ids at once
        /// </summary>
        /// <param name="position">position vbo id</param>
        /// <param name="normal">normal vbo id</param>
        /// <param name="indices">ibo id</param>
        /// <param name="textCoords">texture vbo id</param>
        public void SetBufferIds(int position, int normal, int indices, int textCoords)
        {
            vbo_position = position;
            vbo_normal = normal;
            vbo_text = textCoords;
            ibo_elements = indices;
        }

    }
}
