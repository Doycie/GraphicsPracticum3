﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Template_P3;

namespace template_P3
{
    class Scene1 : Scene
    {
        public override string NAME
        {
            get
            {
                return "DEMO";
            }
        }

        private Entity pot, floor, penguin, penguin2, floor2;                  // a mesh to draw using OpenGL

        private Shader shader_light;
        private Shader shader_sky;
        private Shader shader_fur;

        private Texture woodtex;
        private Texture skyboxtex;
        private Texture furtex;

        private Mesh skyboxmesh;

        Vector3 a = new Vector3(0, 0, 0);

        protected override void LoadScene()
        {
            //load textures
            List<String> a = new List<string>();
            a.Add("ft.png");
            a.Add("bk.png");
            a.Add("up.png");
            a.Add("dn.png");
            a.Add("rt.png");
            a.Add("lf.png");

            skyboxtex = new Texture("../../assets/sky/darkskies_", a);
            woodtex = new Texture("../../assets/wood.png");
            furtex = new Texture("../../assets/fur2.png");

            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_light = new Shader("../../shaders/vs.glsl", "../../shaders/fs_light.glsl");
            shader_sky = new Shader("../../shaders/vs_sky.glsl", "../../shaders/fs_sky.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            shader_fur = new Shader("../../shaders/vs_fur.glsl", "../../shaders/fs_fur.glsl");

            // load entities
            pot = new EntitySkyReflect(new Mesh("../../assets/teapot.obj", 0.3f, 0.3f, 0.3f), shader, woodtex, skyboxtex);
            floor = new EntityFur(new Mesh("../../assets/floor.obj"), shader_fur, furtex);
            floor2 = new EntitySkyReflect(new Mesh("../../assets/floor.obj"), shader, woodtex, skyboxtex);
            penguin = new EntitySkyReflect(new Mesh("../../assets/pin.obj"), shader, woodtex, skyboxtex);
            penguin2 = new EntitySkyReflect(new Mesh("../../assets/pin.obj"), shader, woodtex, skyboxtex);

            //Move and scale entities
            penguin.Move(new Vector3(20.0f, 1.0f, 1.0f));
            penguin2.Move(new Vector3(5.0f, 1.0f, 5.0f));
            floor2.Scale(new Vector3(10.0f, 10.0f, 10.0f));
            floor2.Move(new Vector3(0, 0.0f, 0));

            //Add them to scenegraph
            scenegraph = new SceneGraph();
            scenegraph.AddRootEntity(floor);
            scenegraph.AddEntity(pot, floor);
            scenegraph.AddEntity(penguin, pot);
            scenegraph.AddRootEntity(penguin2);
            scenegraph.AddRootEntity(floor2);

            //Load skybox
            skyboxmesh = new Mesh("../../assets/cube1.obj");

            lights = new List<EntityLight>();
            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(200, 0, 0)));
            lights[0].SetPostition(new Vector3(5, 1, 5));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(0, 250, 0)));
            lights[1].SetPostition(new Vector3(-5, 1, 5));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(0, 0, 200)));
            lights[2].SetPostition(new Vector3(5, 1, -5));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(100, 100, 100)));
            lights[3].SetPostition(new Vector3(-5, 1, -5));

            for (int i = 0; i < lights.Count; i++)
                scenegraph.AddRootEntity(lights[i]);
        }

        public override void Update(long delta_t)
        {
            penguin2.rotation += new Vector3(0, 0, delta_t / 500f);
            floor.rotation = new Vector3(0, (float)Math.Sin(Utility.currentTimeInMilliseconds % 4000 / 2000f * Math.PI), 0);
            lights[0].SetPostition(lights[0].GlobalLocation + new Vector3(0, 0, (float)Math.Sin(Utility.currentTimeInMilliseconds % 4000 / 2000f * Math.PI)));

            PushLightsToShader();
        }

        public override void DrawScene(Camera c)
        {
            //render skybox
            GL.DepthMask(false);
            skyboxmesh.RenderCubeMap(shader_sky, c.CameraRotationMatrix, c.CameraProjMatrix, skyboxtex);
            GL.DepthMask(true);

            //render scenegraph
            scenegraph.Render(c);
        }
    }
}