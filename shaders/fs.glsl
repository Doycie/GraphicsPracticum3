#version 330

const int maxLights = 4;

uniform int lightsAmount = 4;

uniform vec3 lightPos[maxLights];
uniform vec3 lightColor[maxLights];

vec3 ambient = vec3(0.5,0.5,0.5); 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec3 vertex;
uniform sampler2D pixels;		// texture sampler
uniform samplerCube skybox;
uniform vec3 campos;

uniform float specularity;
uniform float diffuse;
uniform float reflection;
uniform float refraction;
uniform float eta;

// shader output
out vec4 outputColor;

// method copied from khronos opengl documentation.
vec3 refract(vec3 I, vec3 N, float eta){
	float k = 1.0 - eta * eta * (1.0 - dot(N, I) * dot(N, I));
	if (k < 0.0)
		return vec3(0, 0, 0);
	else
		return eta * I - (eta * dot(N, I) + sqrt(k)) * N;
}

// fragment shader
void main()
{

	if(texture(pixels,uv).a < 0.1){
		discard;
	}
	vec3 lightspec = vec3(0,0,0);
	vec3 lightlamb = vec3(0,0,0);
	vec3 refr = vec3(0,0,0);
	vec3 refl = vec3(0,0,0);
	vec3 norm = normalize(normal.xyz);
	
	for(int i = 0; i < lightsAmount; i++){
		vec3 lightdir = normalize(lightPos[i] - vertex);
		float lamb = max(dot(lightdir,norm),0.0);
		
		vec3 viewdirection = normalize(-vertex);
		vec3 dir = normalize(lightdir + viewdirection);
		float specular = max(dot(dir, norm), 0.0);
		
		lightspec +=  specularity * lightColor[i].xyz * specular*specular*specular*specular/(( length(lightPos[i] - vertex) *( length(lightPos[i] - vertex))));
		lightlamb +=  diffuse * lightColor[i].xyz * lamb /(( length(lightPos[i] - vertex) *( length(lightPos[i] - vertex)))); 
	}
	
	if (refraction > 0){
		vec3 I = normalize( campos + vertex);
		vec3 R = refract(I,  norm, eta);
		refr = refraction * vec3(texture(skybox, R).xyz);
	}

	if (reflection > 0){
		vec3 I = normalize( campos + vertex);
		vec3 R = reflect(I,  norm );
		refl = reflection * vec3(texture(skybox, R).xyz);
	}

	vec4 total = vec4(lightlamb + lightspec + refr +refl + ambient,1.0);
	outputColor = clamp( total * texture(pixels,uv) ,0.0,1.0);
	
}