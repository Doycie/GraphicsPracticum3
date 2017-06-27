#version 330

bool vignetting = false;
bool chromatic_abberation = false;

// shader input

in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)

uniform float weight[5] = float[] (0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216);

// shader output
out vec4 outputColor;

void main()
{
	float dist = (length(vec2(0.5,0.5)-P ) )/2.0f;
	float red;
	if(!chromatic_abberation)
		red = (texture( pixels, uv ).r);
	else	
		red = (texture( pixels, uv +dist/20.0f).r);
	float green = (texture( pixels, uv).g);
	float blue = (texture( pixels, uv).b);
	
	vec3 vign = vec3(0,0,0);
	
	if(vignetting)
	 vign = vec3(clamp((vec3(1,1,1)*dist),0,1));
	
	outputColor =  vec4( vec3(red,green,blue) - vign,1) ;
}