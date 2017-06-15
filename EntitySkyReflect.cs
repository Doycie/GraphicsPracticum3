using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template_P3;
using OpenTK;


    public class EntitySkyReflect : Entity
    {
            protected Texture skyboxtex;
        
        public EntitySkyReflect(Mesh m, Shader s, Texture tex, Texture sky) : base(m, s, tex)
        {
            skyboxtex = sky;
        }

        public void Render(Matrix4 proj,Matrix4 m, Vector3 campos)
        {
            mesh.Render(shader, proj, m, texture, skyboxtex, campos);
        }
    }

