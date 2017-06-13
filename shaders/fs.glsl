#version 330

const int lightsAmount = 3;

vec3 lightPos[lightsAmount] = {{5,1,5},{-5,1,5}, {5,1,-5}};
vec4 ambient = { 0.8,0.8,0.8,1.0};
vec4 lightColor[lightsAmount] = {{ 0.5,0.0,0.0 , 1.0},{0.0,0.5,0.0,1.0} ,{0.0,0.0,1.0,1.0}};
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec3 vertex;
uniform sampler2D pixels;		// texture sampler


// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	vec4 light = vec4(0,0,0,0);
	for(int i = 0; i < lightsAmount; i++){
		
		vec3 lightdir = lightPos[i] - vertex;
	
		float lamb = max(dot(lightdir,normal.xyz),0.0);
  
		vec3 viewdirection = normalize(-vertex);
		vec3 dir = normalize(lightdir + viewdirection);
		float specular = max(dot(dir, normal.xyz), 0.0);
		
		light += texture( pixels, uv ) * lightColor[i] * lamb * specular;
	}
	
	
	
	
	outputColor = clamp(light + ambient * texture(pixels,uv),0.0,1.0);
	

	//outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
}