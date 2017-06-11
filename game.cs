using System.Diagnostics;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    internal class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.

        private Mesh mesh, floor;                       // a mesh to draw using OpenGL
        private const float PI = 3.1415926535f;         // PI
        private Stopwatch timer;                        // timer for measuring frame duration
        private Shader shader;                          // shader to use for rendering
        private Shader postproc;                        // shader to use for post processing
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
            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            scenegraph = new SceneGraph();
            scenegraph.AddMesh(floor, null);
            scenegraph.AddMesh(mesh, floor);

            camera = new Camera();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
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

            // prepare matrix for vertex shader
            //Matrix4 transform = Matrix4.CreateFromAxisAngle( viewdirection, 0 );

            //  Matrix4 transform = new Matrix4(new Vector4(rightdirection ,0), new Vector4(updirection,0 ),new Vector4(viewdirection,0),new Vector4(0,0,0,1));
            //transform *= Matrix4.CreateTranslation( CamPos );

            // update rotation

            if (useRenderTarget)
            {
                target.Bind();
                scenegraph.Render(camera.getCameraMatrix(), shader, wood);
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                scenegraph.Render(camera.getCameraMatrix(), shader, wood);
            }
            //if (useRenderTarget)
            //{
            //	// enable render target
            //	target.Bind();

            //	// render scene to render target
            //	mesh.Render( shader, transform, wood );
            //	floor.Render( shader, transform, wood );

            //	// render quad
            //	target.Unbind();
            //	quad.Render( postproc, target.GetTextureID() );
            //}
            //else
            //{
            //	// render scene directly to the screen
            //	mesh.Render( shader, transform, wood );
            //	floor.Render( shader, transform, wood );
            //}
        }
    }
} // namespace Template_P3