using System.Diagnostics;
using OpenTK;
// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    internal class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.

        private Entity pot, floor, penguin, sky, penguin2, floor2, cube;                       // a mesh to draw using OpenGL
        private const float PI = 3.1415926535f;         // PI
        private Stopwatch timer;                        // timer for measuring frame duration
        private Shader shader;                          // shader to use for rendering
        private Shader postproc;                        // shader to use for post processing
        private Shader shader_sky;

        private RenderTarget target;                    // intermediate render target
        private ScreenQuad quad;                        // screen filling quad for post processing
        private bool useRenderTarget = true;

        private SceneGraph scenegraph;

        private Camera camera;

        // initialize
        public void Init()
        {
            Texture t = new Texture("../../assets/wood.jpg");
            // load teapot
            pot = new Entity(new Mesh("../../assets/teapot.obj"),t);
            floor = new Entity(new Mesh("../../assets/floor.obj"),t);
            floor2 = new Entity(new Mesh("../../assets/floor.obj"),t);
            penguin = new Entity( new Mesh("../../assets/pin.obj"),t);
            penguin2 = new Entity(new Mesh("../../assets/pin.obj"),t);
            penguin.Move(new Vector3(20.0f,1.0f,1.0f));
            penguin2.Move(new Vector3(5.0f, 1.0f, 1.0f));
            floor2.Scale(new Vector3(10.0f, 10.0f, 10.0f));
            floor2.Move(new Vector3(0, 15.0f, 0));

            t = new Texture("../../assets/cube.png");
            cube = new Entity(new Mesh("../../assets/cube.obj"),t);
            cube.Move(new Vector3(20.0f, 5.0f, 5.0f));

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_sky = new Shader("../../shaders/vs_sky.glsl", "../../shaders/fs_sky.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");

            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            scenegraph = new SceneGraph();
            scenegraph.AddEntity(floor, null);
            scenegraph.AddEntity(pot, floor);
            scenegraph.AddEntity(penguin, pot);
            scenegraph.AddEntity(penguin2, penguin);
            scenegraph.AddEntity(floor2, null);
            scenegraph.AddEntity(cube, null);

            camera = new Camera();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
           // screen.Print("hello world", 2, 2, 0xffff00);
        }

        public void Input(OpenTK.Input.KeyboardState k)
        {
            camera.Input(k);
            
           // sky.SetPostition(camera.camPos);
           // sky.Scale(new Vector3(50.0f, 50.0f, 50.0f));
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
                scenegraph.Render(camera.getCameraMatrix(), shader);
                //sky.Render(shader_sky, camera.getCameraMatrix(), sky.ModelMatrix * camera.cameraModelMatrix(), wood);
                target.Unbind();
             //  int a =screen.GenTexture();
                quad.Render(postproc,target.GetTextureID());
            }
            else
            {
                scenegraph.Render(camera.getCameraMatrix(), shader);
            }
          
        }
    }
} // namespace Template_P3