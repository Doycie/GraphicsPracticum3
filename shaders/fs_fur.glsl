#version 330


vec3 ambient = { 1,1,1}; 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
uniform sampler2D pixels;		// texture sampler

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	
	vec4 total = vec4(vec3(ambient ),1.0);
	outputColor = clamp( total  * texture(pixels,uv) ,0.0,1.0);
	
}