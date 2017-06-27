using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Template_P3;

namespace template_P3
{
    class Scene4 : Scene
    {
        public override string NAME
        {
            get
            {
                return "MEMES";
            }
        }

        private Entity datboi;

        private Shader shader_light;
        private Shader shader_sky;
        private Shader shader_fur;

        private Texture datboitex;

        Vector3 a = new Vector3(0, 0, 0);

        protected override void LoadScene()
        {
            //load textures


          
            datboitex = new Texture("../../assets/dat_boi.png");
          

            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_light = new Shader("../../shaders/vs.glsl", "../../shaders/fs_light.glsl");
            shader_sky = new Shader("../../shaders/vs_sky.glsl", "../../shaders/fs_sky.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            shader_fur = new Shader("../../shaders/vs_fur.glsl", "../../shaders/fs_fur.glsl");

            // load entities
            datboi = new EntityLight(new Mesh("../../assets/datboi2.obj"), shader, datboitex,new Vector3(1,1,1));
           

            //Add them to scenegraph
            scenegraph = new SceneGraph();
            scenegraph.AddRootEntity(datboi);
  

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
          
            lights[0].SetPostition(lights[0].GlobalLocation + new Vector3(0, 0, (float)Math.Sin(Utility.currentTimeInMilliseconds % 4000 / 2000f * Math.PI)));

            PushLightsToShader();
        }

        public override void DrawScene(Camera c)
        {
            //render skybox
            GL.DepthMask(false);
           
            GL.DepthMask(true);

            //render scenegraph
            scenegraph.Render(c);
        }
    }
}
