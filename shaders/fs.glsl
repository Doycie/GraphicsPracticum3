#version 330

const int maxLights = 4;

uniform int lightsAmount = 4;
uniform vec3 lightPos[maxLights];
uniform vec3 lightColor[maxLights];

uniform vec3 ambient;

// shader input
in vec2 uv;						// interpolated texture coordinates
in vec3 normal;					// interpolated normal
in vec3 vertex;

uniform sampler2D tex;		// texture sampler
uniform samplerCube skybox;
uniform vec3 campos;

uniform float specularity;
uniform float diffuse;
uniform float reflection;
uniform float refraction;
uniform float eta;

out vec4 outputColor;

// method copied from khronos opengl documentation. For older openGL versions
vec3 refract(vec3 I, vec3 N, float eta){
	float k = 1.0 - eta * eta * (1.0 - dot(N, I) * dot(N, I));
	if (k < 0.0)
		return vec3(0, 0, 0);
	else
		return eta * I - (eta * dot(N, I) + sqrt(k)) * N;
}

vec3 diffuseSpecularityColor(){
	vec3 outColor = ambient;

	for(int i = 0; i < lightsAmount; i++){
		vec3 lightdir = normalize(lightPos[i] - vertex);
		float lamb = max(dot(lightdir,normal),0.0);
		
		vec3 viewdirection = normalize(-vertex);
		vec3 dir = normalize(lightdir + viewdirection);
		float specular = max(dot(dir, normal), 0.0);
		
		outColor +=  specularity * lightColor[i] * specular*specular*specular*specular/(( length(lightPos[i] - vertex) *( length(lightPos[i] - vertex))));
		outColor +=  diffuse * lightColor[i].xyz * lamb /(( length(lightPos[i] - vertex) *( length(lightPos[i] - vertex)))); 
	}

	return outColor;
}

vec3 refractionColor(){
	vec3 I = normalize( campos + vertex);
	vec3 R = refract(I,  normal, eta);
	return refraction * vec3(texture(skybox, R).xyz);
}

vec3 reflectionColor(){
	vec3 I = normalize( campos + vertex);
	vec3 R = reflect(I,  normal );
	return reflection * vec3(texture(skybox, R).xyz);
}

// fragment shader
void main()
{
	if(texture(tex,uv).w < 0.5){
		discard;
	}

	vec3 lambSpec = diffuseSpecularityColor();
	vec3 refr = refractionColor();
	vec3 refl = reflectionColor();
		
	vec3 total = lambSpec + refr + refl;
	outputColor = vec4(total * texture(tex,uv).xyz, 1.0);
}