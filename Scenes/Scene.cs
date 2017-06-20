using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Template_P3;

namespace template_P3
{
    abstract class Scene
    {
        protected SceneGraph scenegraph;

        protected Shader shader;                          // shader to use for rendering

        int VBO_LightPos;
        int VBO_LightCol;

        public List<EntityLight> lights;

        public Scene()
        {
        }

        protected abstract void LoadScene();

        public void Load()
        {
            LoadScene();

            InitLights();
        }

        protected void InitLights()
        {
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
        }

        public abstract void Draw(Camera c);

        public abstract void DrawPost(ScreenQuad q, RenderTarget t);
    }
}
