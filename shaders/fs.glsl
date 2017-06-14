#version 330

const int lightsAmount = 4;

uniform vec3 lightPos[lightsAmount] = {{5,1,5},{-5,1,5}, {5,1,-5}, {-5,1,-5}};
uniform vec4 lightColor[lightsAmount] = {{ 0.5,0.0,0.0 , 40.0},{0.0,0.5,0.0,50.0} ,{0.0,0.0,1.0,20.0}, {1.0,1.0,1.0,10.0}};

vec3 ambient = { 0.3,0.3,0.3}; 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec3 vertex;
uniform sampler2D pixels;		// texture sampler
uniform samplerCube skybox;
uniform vec3 campos;

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	vec3 lightspec = vec3(0,0,0);
	vec3 lightlamb = vec3(0,0,0);
	vec3 norm = normalize(normal.xyz);
	
	for(int i = 0; i < lightsAmount; i++){
		
		
		vec3 lightdir = normalize(lightPos[i] - vertex);
		float lamb = max(dot(lightdir,norm),0.0);
		
		
		vec3 viewdirection = normalize(-vertex);
		vec3 dir = normalize(lightdir + viewdirection);
		float specular = max(dot(dir, norm), 0.0);
		
		lightspec +=  lightColor[i].xyz * specular*specular*specular*specular * lightColor[i].w /(( length(lightPos[i] - vertex) *( length(lightPos[i] - vertex))));
		lightlamb +=  lightColor[i].xyz * lamb * lightColor[i].w /(( length(lightPos[i] - vertex) *( length(lightPos[i] - vertex)))); 
	}
	
	vec3 I = normalize( campos + vertex);
    vec3 R = refract(I,  norm , 0.9f);
	vec3 refr = vec3(texture(skybox, R).xyz);
	
	vec4 total = vec4(vec3(lightlamb +  lightspec  +ambient + refr),1.0);
	outputColor = clamp( total  * texture(pixels,uv) ,0.0,1.0);
	

	//outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
}