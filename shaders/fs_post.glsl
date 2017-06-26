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

vec3 clampToBrightness(vec3 color){
    float brightness = dot(color, vec3(0.2126, 0.7152, 0.0722));
    if(brightness > 1.0)
        return color;
	return vec3(0,0,0);
}

vec3 bloom (){
    vec2 tex_offset = 1.0 / textureSize(pixels, 0); // gets size of single texel
    vec3 result = clampToBrightness(texture(pixels, uv).rgb) * weight[0]; // current fragment's contribution

    for(int i = 1; i < 5; ++i)
    {
        result += clampToBrightness(texture(pixels, uv + vec2(tex_offset.x * i, 0.0)).rgb) * weight[i];
        result += clampToBrightness(texture(pixels, uv - vec2(tex_offset.x * i, 0.0)).rgb) * weight[i];
    };

	return result;
}

void main()
{
	// retrieve input pixel
	//outputColor = vec4(1.0, 0.0, 1.0, 1.0);

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

	vec3 bloomcolor = bloom();
	
	outputColor =  vec4( vec3(red,green,blue) - vign  + bloomcolor,1) ;
	
	// apply dummy postprocessing effect
	//float dx = P.x - 0.5, dy = P.y - 0.5;
	//float distance = sqrt( dx * dx + dy * dy );
	//outputColor *= sin( distance * 200.0f ) * 0.25f + 0.75f;
}

// EOF