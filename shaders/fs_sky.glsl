#version 330

// shader input
in vec3 uv;						// interpolated texture coordinates
uniform samplerCube TexCube;		// texture sampler

// shader output
out vec4 outputColor;





// fragment shader
void main()
{
	outputColor = texture(TexCube, uv) ;
}