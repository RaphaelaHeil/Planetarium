#version 330

in vec3 normal;
in vec2 vertexTexture;
in vec3 lightDir;
uniform int mode;
uniform sampler2D textureIn;
uniform sampler2D mNormal;
uniform sampler2D bump;
uniform int illu;

out vec4 outputColor;
 
void
main()
{
	
	vec3 l = normalize(lightDir);
	
	float intense = max(dot(normal, l), 0.0);
	vec4 shade = vec4(0.0);
		
	if(intense >= 0){
		shade =	vec4(1.0)*intense;
	}else{
		shade = vec4(0.0)*intense;
	}

				
	if(illu ==1){ //"react" to light
		if(mode == 1){
			outputColor = texture( textureIn, vertexTexture) + shade; //show regular texture
		}else if(mode ==2){
			outputColor = texture(mNormal, vertexTexture) + shade; //show normal map
		}else if(mode == 3){
			outputColor = texture(bump, vertexTexture) + shade; //show bump map
		}else{ //partial "cel" shader

			if(intense>=1){
				outputColor = vec4(1.0,0.0,0.0,1.0)*intense;
			}else if(intense >= 0.60){
				outputColor = vec4(1.0,0.0,0.0,1.0)*intense;
			}else if(intense >= 0.4){
				outputColor = vec4(0.75,0.0,0.0,1.0)*intense;
			}else if(intense >= 0.25){
				outputColor = vec4(0.5,0.0,0.0,1.0)*intense;
			}else if(intense >= 0.0){
				outputColor = vec4(0.25,0.0,0.0,1.0)*intense;
			}else{ //<= 0
				outputColor = shade;
			}
		}
	}else{ //ignore lighting (sun & skybox)
		
		if(mode == 1){ //show regular texture
			outputColor = texture( textureIn, vertexTexture);
		}else if(mode ==2){ //show normal map
			outputColor = texture(mNormal, vertexTexture);
		}else if(mode == 3){ //show bump map
			outputColor = texture(bump, vertexTexture);
		}else{ //show regular texture
			outputColor = texture( textureIn, vertexTexture);
		}
	}
}