#version 330
 
// shader input
uniform float time;
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position
uniform mat4 projmatrix;
uniform mat4 modelmatrix;
uniform mat4 normalmatrix;

out vec3 vertex;
out float vtime;
 
// vertex shader
void main()
{
	
		
	// transform vertex using supplied matrices
	gl_Position = projmatrix  * modelmatrix * vec4(vPosition,  1.0) ;
	
	
	// forward normal and uv coordinate; will be interpolated over triangle
	vtime = time;
	vertex = (modelmatrix * vec4(vPosition,1.0)).xyz;
	
}