﻿using OpenTK;
using Template_P3;
using OpenTK.Graphics.OpenGL;

namespace template_P3
{
    class EntityLight : Entity
    {
        public Vector3 color;

        public EntityLight(Mesh m, Shader s, Texture tex, Vector3 color) : base(m, s, tex)
        {
            this.color = color;
        }

        public override void Render(Camera c, Matrix4 m)
        {
            GL.ProgramUniform3(shader.programID, shader.uniform_color, color);

            mesh.Render(shader, c.getCameraMatrix(), m, texture);
        }
    }
}
