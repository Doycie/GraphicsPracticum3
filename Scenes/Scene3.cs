using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Template_P3;

namespace template_P3
{
    class Scene3 : Scene
    {
        public override string NAME
        {
            get
            {
                return "LightBox";
            }
        }

        private Entity box;                  // a mesh to draw using OpenGL

        private Shader shader_light;
        private Shader shader_cloud;

        private Texture woodtex;
        private Texture skyboxtex;

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

            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_light = new Shader("../../shaders/vs.glsl", "../../shaders/fs_light.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            shader_cloud = new Shader("../../shaders/vs_cloud.glsl", "../../shaders/fs_cloud.glsl");

            //Load skybox
            skyboxmesh = new Mesh("../../assets/cube1.obj");

            // load entities
            box = new EntitySkyReflect(new Mesh("../../assets/pin.obj"), shader, woodtex, skyboxtex);
            box.scale = new Vector3(5, 5, 5);

            //Add them to scenegraph
            scenegraph = new SceneGraph();
            scenegraph.AddRootEntity(box);

            lights = new List<EntityLight>();

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(200, 0, 0)));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(0, 250, 0)));
            lights[1].SetPostition(new Vector3(-5, 0, 0));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(0, 0, 200)));
            lights[2].SetPostition(new Vector3(10, 0, 0));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(200, 200, 200)));
            lights[3].SetPostition(new Vector3(0, 10, 3));

            scenegraph.AddRootEntity(lights[0]);

            for (int i = 1; i < lights.Count; i++)
                scenegraph.AddEntity(lights[i], box);
        }

        public override void Update(long delta_t)
        {
            float time = (float) (Utility.currentTimeInMilliseconds % 4000 / 2000f * Math.PI);

            GL.ProgramUniform1(shader_cloud.programID, shader_cloud.uniform_time, (Utility.currentTimeInMilliseconds % 100000) / 8000f);

            box.rotation = new Vector3(0, time, 0);

            lights[2].translation = new Vector3((float)(4 * Math.Sin(time)) + 10, 0.0f, 0.0f);

            lights[0].translation = new Vector3(23 * (float)(Math.Sin(2 * time)), 16 * (float)(Math.Sin(time)), 33 * (float)(Math.Sin(time)));

            PushLightsToShader();
        }

        public override void DrawScene(Camera c)
        {
            //render skybox
            GL.DepthMask(false);
            skyboxmesh.RenderCubeMap(shader_cloud, c.getCameraRotationMatrix(), c.getCameraProjMatrix(), null);
            GL.DepthMask(true);

            //render scenegraph
            scenegraph.Render(c);
        }
    }
}
