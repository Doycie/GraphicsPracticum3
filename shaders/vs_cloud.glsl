#version 330

uniform float time;

in vec3 vPosition;			// untransformed vertex position
uniform mat4 projmatrix;
uniform mat4 modelmatrix;

// shader output
out float vtime;
out vec3 vertex;

// vertex shader
void main()
{
		// transform vertex using supplied matrices
	gl_Position = modelmatrix * projmatrix * vec4(vPosition, 1.0);

	
	// forward normal and uv coordinate; will be interpolated over triangle
	vtime = time;
	vertex = vPosition;
	
}