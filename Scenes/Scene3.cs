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
                return "Outdoor Sky Basketball";
            }
        }

        private Entity pinguin, floor;                  // a mesh to draw using OpenGL

        private Shader shader_light;
        private Shader shader_cloud;
        private Shader shader_fur;

        private Texture woodtex;
        private Texture skyboxtex;
        private Texture furtex;

        private Mesh skyboxmesh;

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
            shader_fur = new Shader("../../shaders/vs_fur.glsl", "../../shaders/fs_fur.glsl");

            furtex = new Texture("../../assets/fur2.png");

            //Load skybox
            skyboxmesh = new Mesh("../../assets/cube1.obj");

            // load entities
            pinguin = new EntitySkyReflect(new Mesh("../../assets/pin.obj"), shader, woodtex, skyboxtex);
            pinguin.scale = new Vector3(0.25f, 0.25f, 0.25f);

            floor = new EntityFur(new Mesh("../../assets/floor.obj"), shader_fur, furtex);
            floor.scale = new Vector3(5, 5, 5);
            floor.Move(new Vector3(0, 0, -10f));

            //Add them to scenegraph
            scenegraph = new SceneGraph();
            scenegraph.AddRootEntity(floor);
            scenegraph.AddEntity(pinguin, floor);

            //Load lightning

            lights = new List<EntityLight>();

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(600, 600, 600)));
            lights[0].SetPostition(new Vector3(0, 10, 3));

            lights.Add(new EntityLight(new Mesh("../../assets/sphere.obj"), shader_light, null, new Vector3(250, 0, 0)));
            lights[1].scale = new Vector3(0.5f, 0.5f, 0.5f);
            lights[1].SetPostition(new Vector3(3.5f, 0, 0.6f));

            ambient = new Vector3(0.0f, 0.0f, 0.8f);

            //Add the lightning to the scenegraph
            scenegraph.AddEntity(lights[0], floor);
            scenegraph.AddEntity(lights[1], pinguin);
        }

        public override void Update(long delta_t)
        {
            float time = (float)(Utility.currentTimeInMilliseconds % 4000 / 2000f * Math.PI);

            GL.ProgramUniform1(shader_cloud.programID, shader_cloud.uniform_time, (Utility.currentTimeInMilliseconds % 100000) / 8000f);

            pinguin.translation = new Vector3((float)(4 * Math.Sin(2 * time)), -2.5f, (float)(20 * Math.Sin(time)));

            lights[1].translation = new Vector3(7f, (float)(7f * Math.Abs((Math.Cos(time * 2)))) - 7f, 0.6f);

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