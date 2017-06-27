6051529 Dennis Arets
5910137 Cody Arets
5937949 Faris van Hien

Controls:
  Control the camera with WASD and rotate it with the arrow keys, to switch scenes press the number keys.
  Turning on chromatic abberation can be done with C to turn it off use F.
  Turning on vignetting can be done with V to turn if off use G

Features:
  Besides the required features several extras have been addded:
	-Support for multiple scenes, switch between scenes by pressing the number keys in the application. 
	-The lights are dynamic and can be added easily. They are defined in C# and pushed to the shader.
	-Cube mapping has been added. See the skybox which is rendered in 2 scenes.
	-Reflection and Refraction for the skybox have been added, they can be modified when loading the meshes. 
		The teapot in the first scene is reflective and refractive. Note older opengl versions do not support
  		the method reflect in GLSL so this was taken from the documentation as specified in fs.glsl
	-Vignetting and chromatic aberration are added. they can be turned on in the post_fs.glsl by setting their bools to true
	-A cloud rendering shader see scene 3 ( using brownian noise and perlin)
	

Used materials:
	-refract method in fs.glsl from khronos opengl documentation.
	-the sky images come from [WTF?]Mr-Gibs see the readme in the assets folder for futher information
	-Dat-boi by Douwef1 is licensed under CC Attribution 
	-the mario kart models were downloaded from models-resource.com :
		-Mario: https://www.models-resource.com/3ds/nintendogscats/model/15991/
		-Peach: https://www.models-resource.com/3ds/nintendogscats/model/15971/
		-Yoshi: https://www.models-resource.com/3ds/nintendogscats/model/15992/
		-Map: https://www.models-resource.com/nintendo_64/mariokart64/model/17213/
		-Star: https://www.models-resource.com/gamecube/luigismansion/model/6896/
