#version 330
 
 vec3 lightPos = {0,20,0};
 
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
	vec3 ab = lightPos - vertex;
	

    outputColor = texture( pixels, uv ) * max(dot(ab,normal.xyz),0.0);

	//outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
}