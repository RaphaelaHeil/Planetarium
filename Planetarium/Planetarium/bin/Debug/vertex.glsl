#version 330
 
in vec3 vPosition;
in vec3 vNormal;
in vec2 vTexture;
out vec2 vertexTexture;
out vec3 normal;
out vec3 lightDir;
uniform mat4 mModel;
uniform mat4 mProjection;
uniform sampler2D mNormal;
uniform sampler2D bump;

 
void
main()
{
	vec4 rotated = mModel * vec4(vPosition, 1.0);

	normal = normalize(vNormal);
	
	lightDir = vec3(vec4(0,0,0,1) - rotated);
	
	gl_Position = mProjection* mModel *  vec4(vPosition, 1.0); //transform vector to receive actual position
	
	vertexTexture = vTexture;
}