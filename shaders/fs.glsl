﻿#version 330
 
vec3 lightPos = {0,20,0};
vec4 ambient = { 0.2,0.2,0.2,1.0};
vec4 lightColor = { 1.0,0.2,0.2 , 1.0};
 
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
	vec3 lightdir = lightPos - vertex;
	
	float lamb = max(dot(lightdir,normal.xyz),0.0);
  
	vec3 viewdirection = normalize(-vertex);
    vec3 dir = normalize(lightdir + viewdirection);
    float specular = max(dot(dir, normal.xyz), 0.0);
	//specular *= specular * specular * specular;
	
	
	outputColor = clamp(texture( pixels, uv ) * lightColor * lamb * specular + ambient * texture(pixels,uv),0.0,1.0);
	

	//outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
}