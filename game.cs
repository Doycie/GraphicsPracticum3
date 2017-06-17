using System;
using System.Diagnostics;
using template_P3;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    internal class Game
    {
        double lastUpdateTime = -1;

        // member variables
        public Surface screen;                  // background surface for printing etc.

        Scene scene;

        private const float PI = 3.1415926535f;         // PI

        private Stopwatch timer;                        // timer for measuring frame duration

        private RenderTarget target;                    // intermediate render target
        private ScreenQuad quad;                        // screen filling quad for post processing
        private bool useRenderTarget = true;

        private Camera camera;

        // initialize
        public void Init()
        {
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            camera = new Camera();

            scene = new Scene1();
            scene.Load();

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            // screen.Print("hello world", 2, 2, 0xffff00);

            if (!timer.IsRunning)
                timer.Start();

            scene.Update(timer.ElapsedMilliseconds);

            timer.Reset();
            timer.Start();
        }

        public void Input(OpenTK.Input.KeyboardState k)
        {
            camera.Input(k);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            if (useRenderTarget)
            {
                target.Bind();

                scene.Draw(camera);

                target.Unbind();

                scene.DrawPost(quad, target);

            }
        }
    }
} // namespace Template_P3