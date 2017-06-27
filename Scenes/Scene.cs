using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;
using Template_P3;

namespace template_P3
{
    abstract class Scene
    {
        //Name for the scene, must be uniquely identifying for scenes.
        public abstract string NAME
        {
            get;
        }

        protected SceneGraph scenegraph;
        protected Shader shader;                          // shader to use for rendering that includes lightning.
        protected Shader postproc;                          // shader to use for post processing

        ScreenQuad blendQuad;

        public List<EntityLight> lights;

        public Vector3 ambient = new Vector3(0.2f, 0.2f, 0.2f);

        public Scene()
        {
            blendQuad = new ScreenQuad();
        }

        protected abstract void LoadScene();

        public void Load()
        {
            LoadScene();

            PushLightsToShader();
        }

        public virtual void Input(KeyboardState k)
        {
            if (k.IsKeyDown(Key.C))
                GL.ProgramUniform1(postproc.programID, postproc.uniform_chromatic_abberation, 1);
            else if (k.IsKeyDown(Key.F))
                GL.ProgramUniform1(postproc.programID, postproc.uniform_chromatic_abberation, 0);
            if (k.IsKeyDown(Key.V))
                GL.ProgramUniform1(postproc.programID, postproc.uniform_vignetting, 1);
            else if (k.IsKeyDown(Key.G))
                GL.ProgramUniform1(postproc.programID, postproc.uniform_vignetting, 0);
        }

        public abstract void Update(long delta_t);

        public void PushLightsToShader()
        {
            float[] light_position = new float[lights.Count * 3];
            float[] light_color = new float[lights.Count * 3];

            for (int i = 0; i < lights.Count; i++)
            {
                light_position[i * 3] = lights[i].GlobalLocation.X;
                light_position[i * 3 + 1] = lights[i].GlobalLocation.Y;
                light_position[i * 3 + 2] = lights[i].GlobalLocation.Z;

                light_color[i * 3] = lights[i].color.X;
                light_color[i * 3 + 1] = lights[i].color.Y;
                light_color[i * 3 + 2] = lights[i].color.Z;
            }

            GL.ProgramUniform1(shader.programID, shader.uniform_lightsAmount, lights.Count);

            GL.ProgramUniform3(shader.programID, shader.uniform_lightPos, lights.Count, light_position);

            GL.ProgramUniform3(shader.programID, shader.uniform_lightColor, lights.Count, light_color);

            GL.ProgramUniform3(shader.programID, shader.uniform_ambient, ambient);
        }

        public void Draw(Camera c)
        {
            DrawBuffersEnum[] buffers =
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1
            };

            GL.DrawBuffers(2, buffers);

            DrawScene(c);
        }

        public abstract void DrawScene(Camera c);

        public virtual void DrawPost(ScreenQuad q, RenderTarget t)
        {
            q.Render(postproc, t.GetTextureID());
        }
    }
}