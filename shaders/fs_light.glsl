#version 330

layout (location = 0) out vec4 outputColor;
layout (location = 1) out vec4 brightness;  

uniform vec3 color = vec3(1.0, 0.0, 0.0);

void main(){
	outputColor = vec4(color, 1.0);
	brightness = vec4(color, 1.0);
}
