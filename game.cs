using OpenTK;
using System;
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

        private Vector3 CamPos = new Vector3(0, 0, 15);
        private Vector3 viewdirection = new Vector3(0, 0, -1);
        private Vector3 rightdirection = new Vector3(1, 0, 0);
        private Vector3 updirection = new Vector3(0, 1, 0);

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
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
        }

        public void RotateAroundX(float angle)
        {
            viewdirection = viewdirection * (float)Math.Cos(angle) + updirection * (float)Math.Sin(angle);
            viewdirection.Normalize();
            updirection = -Vector3.Cross(viewdirection, rightdirection);
        }

        public void RotateAroundY(float angle)
        {
            viewdirection = viewdirection * (float)Math.Cos(angle) - rightdirection * (float)Math.Sin(angle);
            viewdirection.Normalize();
            rightdirection = Vector3.Cross(viewdirection, updirection);
        }

        public void RotateAroundZ(float angle)
        {
            rightdirection = rightdirection * (float)Math.Cos(angle) + updirection * (float)Math.Sin(angle);
            rightdirection.Normalize();
            updirection = -Vector3.Cross(viewdirection, rightdirection);
        }
    
        public void Input(OpenTK.Input.KeyboardState k)
        {
            if (k.IsKeyDown(OpenTK.Input.Key.W))
            {
                CamPos += viewdirection;
            }
            if (k.IsKeyDown(OpenTK.Input.Key.S))
            {
                CamPos -= viewdirection;
            }
            if (k.IsKeyDown(OpenTK.Input.Key.A))
            {
                CamPos += rightdirection;
            }
            if (k.IsKeyDown(OpenTK.Input.Key.D))
            {
                CamPos -= rightdirection;
            }
            if (k.IsKeyDown(OpenTK.Input.Key.Up))
            {
                RotateAroundX(-0.1f);
            }
            if (k.IsKeyDown(OpenTK.Input.Key.Down))
            {
                RotateAroundX(0.1f);
            }

            if (k.IsKeyDown(OpenTK.Input.Key.Right))
            {
                RotateAroundY(0.1f);
            }
            if (k.IsKeyDown(OpenTK.Input.Key.Left))
            {
                RotateAroundY(-0.1f);
            }

            if (k.IsKeyDown(OpenTK.Input.Key.Q))
            {
                RotateAroundZ(0.1f);
            }
            if (k.IsKeyDown(OpenTK.Input.Key.E))
            {
                RotateAroundZ(-0.1f);
            }
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
            Matrix4 transform = Matrix4.CreateTranslation(CamPos);
            transform *= new Matrix4(new Vector4(rightdirection, 0), new Vector4(updirection, 0), new Vector4(viewdirection, 0), new Vector4(0, 0, 0, 1));
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            //  Matrix4 transform = new Matrix4(new Vector4(rightdirection ,0), new Vector4(updirection,0 ),new Vector4(viewdirection,0),new Vector4(0,0,0,1));
            //transform *= Matrix4.CreateTranslation( CamPos );

            // update rotation

            if (useRenderTarget)
            {
                target.Bind();
                scenegraph.Render(transform, shader, wood);
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                scenegraph.Render(transform, shader, wood);
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