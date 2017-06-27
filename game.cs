using OpenTK;
using System;
using System.Diagnostics;
using template_P3;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    internal class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.

        private GameWindow window;

        Scene scene;

        private const float PI = 3.1415926535f;         // PI

        private Stopwatch timer;                        // timer for measuring frame duration

        private RenderTarget target;                    // intermediate render target
        private ScreenQuad quad;                        // screen filling quad for post processing
        private bool useRenderTarget = true;

        private Camera camera;

        public Game(GameWindow window)
        {
            this.window = window;
        }

        // initialize
        public void Init()
        {
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            camera = new Camera();

            SwitchScene(new Scene1());

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

            if (k.IsKeyDown(OpenTK.Input.Key.Number1))
                SwitchScene(new Scene1());
            else if (k.IsKeyDown(OpenTK.Input.Key.Number2))
                SwitchScene(new Scene2());
            else if (k.IsKeyDown(OpenTK.Input.Key.Number4))
                SwitchScene(new Scene4());
        }

        public void SwitchScene(Scene scene)
        {
            if(this.scene == null || this.scene.NAME != scene.NAME)
            {
                this.scene = scene;
                scene.Load();
                window.Title = "Codengine: scene " + scene.NAME;
            }
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