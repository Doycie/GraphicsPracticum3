#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position
uniform mat4 projmatrix;
uniform mat4 modelmatrix;
uniform mat4 normalmatrix;

// shader output
out vec4 normal;			// transformed vertex normal
out vec2 uv;
out vec3 vertex;			

 
// vertex shader
void main()
{
	// transform vertex using supplied matrices
	gl_Position = projmatrix * modelmatrix * vec4(vPosition, 1.0) ;
	
	normal = normalmatrix * vec4( vNormal, 0.0f );
	// forward normal and uv coordinate; will be interpolated over triangle
	uv = vUV;
	vertex = vec3(modelmatrix * vec4(vPosition,1.0));
}