#version 330

// shader input

in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)

// shader output
out vec4 outputColor;

void main()
{
	// retrieve input pixel
	//outputColor = vec4(1.0, 0.0, 1.0, 1.0);
	float dist = (length(vec2(0.5,0.5)-P ) )/50;
	float red = (texture( pixels, uv ).r);
	float green = (texture( pixels, uv).g);
	float blue = (texture( pixels, uv).b);

	outputColor =  vec4( vec3(red,green,blue) - clamp((vec3(1,1,1)*dist),0,1),1) ;
	
	
	
	// apply dummy postprocessing effect
	//float dx = P.x - 0.5, dy = P.y - 0.5;
	//float distance = sqrt( dx * dx + dy * dy );
	//outputColor *= sin( distance * 200.0f ) * 0.25f + 0.75f;
	
}

// EOF