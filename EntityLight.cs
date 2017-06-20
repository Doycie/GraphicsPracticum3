using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Template_P3;

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
            
        }
    }
}
