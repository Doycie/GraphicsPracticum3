#version 330

// shader input
in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D tex;		// input texture (1St pass render target)

uniform bool vignetting = false;
uniform bool chromatic_abberation = false;

// shader output
out vec4 outputColor;

void main()
{
	float dist = (length(vec2(0.5,0.5)-P ) )/2.0f;

	float red;
	red = (texture(tex, uv).r);

	float green = (texture(tex, uv).g);
	float blue = (texture(tex, uv).b);
	
	if(chromatic_abberation)
		red = (texture(tex, uv + dist / 20.0F).r);

	vec3 vign = vec3(0,0,0);
	
	if(vignetting)
		vign = vec3(clamp((vec3(1,1,1)*dist),0,1));
	
	outputColor =  vec4( vec3(red,green,blue) - vign,1) ;
}