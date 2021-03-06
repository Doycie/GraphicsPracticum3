﻿#version 330
 
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

// offset of the overlaying fur textures
uniform float shellOffset;
 
// vertex shader
void main()
{
	// transform vertex using supplied matrices
	gl_Position = projmatrix * modelmatrix * vec4(vPosition + shellOffset * vNormal, 1.0) ;
	
	// forward normal and uv coordinate; will be interpolated over triangle
	normal = normalmatrix * vec4( vNormal, 0.0f );
	uv = vUV;
}