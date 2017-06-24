using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Template_P3;

namespace template_P3
{
    class Scene2 : Scene
    {
        private Entity pot, floor, penguin, penguin2, floor2,
            kart1, kart2, kart3, mario, peach, yoshi, wheel1, wheel2, wheel3,
            map, star;                 // a mesh to draw using OpenGL

        private Shader postproc;                        // shader to use for post processing
        private Shader shader_sky;
        private Shader shader_fur;

        private Texture woodtex;
        private Texture skyboxtex;
        private Texture furtex;
        private Texture mkartex, pkartex, ykartex, martex, peatex, yostex, wheeltex, nultex,
            maptex1, startex;

        private Mesh skyboxmesh;

        Vector3 a = new Vector3(0, 0, 0);

        protected override void LoadScene() {
            //load textures
            List<String> a = new List<string>();
            a.Add("ft.png");
            a.Add("bk.png");
            a.Add("up.png");
            a.Add("dn.png");
            a.Add("rt.png");
            a.Add("lf.png");

            skyboxtex = new Texture("../../assets/sky/darkskies_", a);
            woodtex = new Texture("../../assets/wood.jpg");
            furtex = new Texture("../../assets/furt.png");
            martex = new Texture("../../assets/Mario Kart/Mario/Tex/mariotex.png");
            peatex = new Texture("../../assets/Mario Kart/Peach/Tex/peachtex.png");
            yostex = new Texture("../../assets/Mario Kart/Yoshi/Tex/yoshitex.png");
            mkartex = new Texture("../../assets/Mario Kart/Mario/Tex/karttex.png");
            pkartex = new Texture("../../assets/Mario Kart/Peach/Tex/karttex.png");
            ykartex = new Texture("../../assets/Mario Kart/Yoshi/Tex/karttex.png");
            wheeltex = new Texture("../../assets/Mario Kart/Mario/Tex/wheeltex.png");
            maptex1 = new Texture("../../assets/Mario Kart/Maps/KPBeach/236F857A_c.png");
            nultex = new Texture("../../assets/null.png");
            startex = new Texture("../../assets/Mario Kart/Star/Mario's Star Image.png");

            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_sky = new Shader("../../shaders/vs_sky.glsl", "../../shaders/fs_sky.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            shader_fur = new Shader("../../shaders/vs_fur.glsl", "../../shaders/fs_fur.glsl");

            // load entities
            kart1 = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Kart.obj"), shader, mkartex, skyboxtex);
            mario = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Mario.obj"), shader, martex, skyboxtex);
            wheel1 = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Wheels.obj"), shader, wheeltex, skyboxtex);
            kart2 = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Kart.obj"), shader, pkartex, skyboxtex);
            peach = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Peach/Peach.obj"), shader, peatex, skyboxtex);
            wheel2 = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Wheels.obj"), shader, wheeltex, skyboxtex);
            kart3 = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Kart.obj"), shader, ykartex, skyboxtex);
            yoshi = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Yoshi/Yoshi.obj"), shader, yostex, skyboxtex);
            wheel3 = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Mario/obj/Wheels.obj"), shader, wheeltex, skyboxtex);
            floor2 = new EntitySkyReflect(new Mesh("../../assets/floor.obj"), shader, nultex, skyboxtex);
            floor = new EntityFur(new Mesh("../../assets/floor.obj"), shader_fur, furtex);
            map = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Maps/KPBeach/KoopaTroopaBeach.obj"), shader, maptex1, skyboxtex);
            star = new EntitySkyReflect(new Mesh("../../assets/Mario Kart/Star/Star.obj"), shader, startex, skyboxtex);


            //Move and scale entities
            map.Move(new Vector3(0, -2, -1));
            floor.Move(new Vector3(0, 0, -4));
            kart1.Rotate(new Vector3(1.57f, 0.0f, 1.57f));
            kart1.Move(new Vector3(0, -2, -3));
            kart2.Rotate(new Vector3(1.57f, 0.0f, 3.14f));
            kart2.Move(new Vector3(3, -2, 0));
            kart3.Rotate(new Vector3(1.57f, 0.0f, 4.71f));
            kart3.Move(new Vector3(0, -2, 3));
            star.Scale(new Vector3(.5f, .5f, .5f));
            star.Move(new Vector3(0, -1, 0));

            //Add them to scenegraph
            scenegraph = new SceneGraph();
            scenegraph.AddRootEntity(map);
            scenegraph.AddRootEntity(floor);
            scenegraph.AddEntity(floor2, floor);
            scenegraph.AddEntity(kart1, floor2);
            scenegraph.AddEntity(mario, kart1);
            scenegraph.AddEntity(wheel1, kart1);
            scenegraph.AddEntity(kart2, floor2);
            scenegraph.AddEntity(peach, kart2);
            scenegraph.AddEntity(wheel2, kart2);
            scenegraph.AddEntity(kart3, floor2);
            scenegraph.AddEntity(yoshi, kart3);
            scenegraph.AddEntity(wheel3, kart3);
            scenegraph.AddEntity(star, floor2);


            //Load skybox
            skyboxmesh = new Mesh("../../assets/cube1.obj");

            lights = new List<EntityLight>();
            lights.Add(new EntityLight(null, null, null, new Vector3(20, 0, 0)));
            lights[0].SetPostition(new Vector3(5, 1, -3));

            lights.Add(new EntityLight(null, null, null, new Vector3(0, 0, 20)));
            lights[1].SetPostition(new Vector3(5, 1, -5));

            lights.Add(new EntityLight(null, null, null, new Vector3(20, 20, 20)));
            lights[2].SetPostition(new Vector3(0, 2, -4));

        }
        public override void Update(long delta_t)
        {
            lights[0].SetPostition(lights[0].GlobalLocation +  new Vector3(0, 0, (float)Math.Sin(Utility.currentTimeInMilliseconds % 4000 / 2000f * Math.PI)));
            if((Utility.currentTimeInMilliseconds % 2000) > 1000)
            {
                floor.Move(new Vector3(-.001f, 0, -.0005f));
                star.Move(new Vector3(0,-.02f,0));
            }
            else
            {
                floor.Move(new Vector3(.001f, 0, .0005f));
                star.Move(new Vector3(0, .02f, 0));
            }

            floor2.rotation += new Vector3(0, delta_t / 1000f, 0);
            PushLightsToShader();
        }

        public override void Draw(Camera c)
        {
            //render skybox
            GL.DepthMask(false);
            skyboxmesh.RenderCubeMap(shader_sky, c.getCameraRotationMatrix(), c.getCameraProjMatrix(), skyboxtex);
            GL.DepthMask(true);

            //render scenegraph
            scenegraph.Render(c);
        }

        public override void DrawPost(ScreenQuad q, RenderTarget t)
        {
            q.Render(postproc, t.GetTextureID());
        }
    }
}
