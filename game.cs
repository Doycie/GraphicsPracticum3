using System.Diagnostics;
using OpenTK;
using System.Collections.Generic;
using System;
using OpenTK.Graphics.OpenGL;
using System.Collections;
// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    internal class Game
    {
        float time = 0;

        // member variables
        public Surface screen;                  // background surface for printing etc.

        private Entity pot, floor, penguin, penguin2, floor2;                  // a mesh to draw using OpenGL
        private const float PI = 3.1415926535f;         // PI
        private Stopwatch timer;                        // timer for measuring frame duration
        private Shader shader;                          // shader to use for rendering
        private Shader postproc;                        // shader to use for post processing
        private Shader shader_sky;
        private Shader shader_fur;

        private RenderTarget target;                    // intermediate render target
        private ScreenQuad quad;                        // screen filling quad for post processing
        private bool useRenderTarget = true;

        private SceneGraph scenegraph;
        private Camera camera;

        private Texture woodtex;
        private Texture skyboxtex;
        private Texture furtex;

        private Mesh skyboxmesh;



        float a = 0.0f;
        // initialize
        public void Init()
        {
            //load textures
            List<String> a = new List<string>();
            a.Add("ft.png");
            a.Add("bk.png");
            a.Add("up.png");
            a.Add("dn.png");
            a.Add("rt.png");
            a.Add("lf.png");

            skyboxtex = new Texture("../../assets/sky/darkskies_",  a);
            woodtex = new Texture("../../assets/wood.png");
            furtex = new Texture("../../assets/fur2.png");

            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_sky = new Shader("../../shaders/vs_sky.glsl", "../../shaders/fs_sky.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            shader_fur = new Shader("../../shaders/vs_fur.glsl", "../../shaders/fs_fur.glsl");

            // load entities
            pot = new EntityFur(new Mesh("../../assets/teapot.obj"),shader_fur,furtex);
            floor = new EntityFur(new Mesh("../../assets/floor.obj"), shader_fur, furtex);
            floor2 = new EntitySkyReflect(new Mesh("../../assets/floor.obj"), shader, woodtex, skyboxtex);
            penguin = new EntitySkyReflect( new Mesh("../../assets/pin.obj"), shader, woodtex, skyboxtex);
            penguin2 = new EntitySkyReflect(new Mesh("../../assets/pin.obj"), shader, woodtex, skyboxtex);

            //Move and scale entities
            penguin.Move(new Vector3(20.0f, 1.0f, 1.0f));
            penguin2.Move(new Vector3(5.0f, 1.0f, 5.0f));
            floor2.Scale(new Vector3(10.0f, 10.0f, 10.0f));
            floor2.Move(new Vector3(0, 0.0f, 0));

            //Add them to scenegraph
            scenegraph = new SceneGraph();
            scenegraph.AddEntity(floor, null);
            scenegraph.AddEntity(pot, floor);
            scenegraph.AddEntity(penguin, pot);
            scenegraph.AddEntity(penguin2, null);
            scenegraph.AddEntity(floor2, null);

            //Load skybox
            skyboxmesh = new Mesh("../../assets/cube1.obj");

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();


            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            camera = new Camera();


        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            // screen.Print("hello world", 2, 2, 0xffff00);

            time += 0.001f;
            GL.ProgramUniform1(shader_sky.programID, shader_sky.attribute_ftime, time);
        }

        public void Input(OpenTK.Input.KeyboardState k)
        {
            camera.Input(k);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            if (useRenderTarget)
            {
                target.Bind();

                //render skybox
                GL.DepthMask(false);       
                skyboxmesh.RenderCubeMap(shader_sky, camera.getCameraRotationMatrix(), camera.getCameraProjMatrix(), skyboxtex);
                GL.DepthMask(true);

                //render scenegraph
                scenegraph.Render(camera.getCameraMatrix(),  camera.getCameraLocation());
        
               
                target.Unbind();
                quad.Render(postproc,target.GetTextureID());
            }
       
          
        }
    }
} // namespace Template_P3