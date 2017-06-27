6051529 Dennis Arets
5910137 Cody Arets
	Faris van Hien

Besides the required features several extras have been addded:
	-Support for multiple scenes, switch between scenes by pressing the number keys in the application. 
	-The lights are dynamic and can be added easily. They are defined in C# and pushed to the shader.
	-Cube mapping has been added. See the skybox which is rendered in both scenes.
	-Reflection and Refraction for the skybox have been added, they can be modified when loading the meshes. 
		The teapot in the first scene is reflective and refractive. Note older opengl versions do not support
  		the method reflect in GLSL so this was taken from the documentation as specified in fs.glsl
	-Vignetting and chromatic aberration are added. they can be turned on in the post_fs.glsl by setting their bools to true
	-
	-

Used materials:
	-refract method in fs.glsl from khronos opengl documentation.
	-the sky images come from [WTF?]Mr-Gibs see the readme in the assets folder for futher information
