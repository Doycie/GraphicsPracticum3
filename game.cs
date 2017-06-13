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

        private Mesh mesh, floor, penguin, sky, penguin2, floor2;                       // a mesh to draw using OpenGL
        private const float PI = 3.1415926535f;         // PI
        private Stopwatch timer;                        // timer for measuring frame duration
        private Shader shader;                          // shader to use for rendering
        private Shader postproc;                        // shader to use for post processing
        private Shader shader_sky;
        private Texture wood;                           // texture to use for rendering
        private RenderTarget target;                    // intermediate render target
        private ScreenQuad quad;                        // screen filling quad for post processing
        private bool useRenderTarget = true;

        private SceneGraph scenegraph;

        private Camera camera;

        // initialize
        public void Init()
        {
            // load teapot
            mesh = new Mesh("../../assets/teapot.obj");
            floor = new Mesh("../../assets/floor.obj");
            floor2 = new Mesh("../../assets/floor.obj");
            penguin = new Mesh("../../assets/pin.obj");
            penguin2 = new Mesh("../../assets/pin.obj");
            sky = new Mesh("../../assets/sky.obj");

            penguin.Move(new Vector3(20.0f,1.0f,1.0f));
            penguin2.Move(new Vector3(5.0f, 1.0f, 1.0f));
            sky.Scale(new Vector3(50.0f, 50.0f, 50.0f));
            floor2.Scale(new Vector3(10.0f, 10.0f, 10.0f));

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            shader_sky = new Shader("../../shaders/vs_sky.glsl", "../../shaders/fs_sky.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            scenegraph = new SceneGraph();
            scenegraph.AddMesh(floor, null);
            scenegraph.AddMesh(mesh, floor);
            scenegraph.AddMesh(penguin, mesh);
            scenegraph.AddMesh(penguin2, penguin);
            scenegraph.AddMesh(floor2, null);

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
                scenegraph.Render(camera.getCameraMatrix(), shader, wood);
                //sky.Render(shader_sky, camera.getCameraMatrix(), sky.ModelMatrix * camera.cameraModelMatrix(), wood);
                target.Unbind();
             //  int a =screen.GenTexture();
                quad.Render(postproc,target.GetTextureID());
                
            }
            else
            {
                scenegraph.Render(camera.getCameraMatrix(), shader, wood);
            }
          
        }
    }
} // namespace Template_P3