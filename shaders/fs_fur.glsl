#version 330

const int lightsAmount = 4;

uniform vec3 lightPos[lightsAmount] = {{5,1,5},{-5,1,5}, {5,1,-5}, {-5,1,-5}};
uniform vec4 lightColor[lightsAmount] = {{ 0.5,0.0,0.0 , 40.0},{0.0,0.5,0.0,50.0} ,{0.0,0.0,1.0,20.0}, {1.0,1.0,1.0,10.0}};

vec3 ambient = { 1,1,1}; 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec3 vertex;
uniform sampler2D pixels;		// texture sampler
uniform samplerCube skybox;
uniform vec3 campos;

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	
	vec4 total = vec4(vec3(ambient ),1.0);
	outputColor = clamp( total  * texture(pixels,uv) ,0.0,1.0);
	

	//outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
}