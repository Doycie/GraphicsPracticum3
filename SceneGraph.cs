using OpenTK;
using OpenTK.Graphics.ES20;
using System.Collections.Generic;

namespace Template_P3
{
    public class SceneGraph
    {
        const float PI = 3.1415926535f;
        float a = 0;
        public List<Entity> entityList;

        public SceneGraph()
        {
            entityList = new List<Entity>();
        }

        public void AddEntity(Entity e, Entity p)
        {
            e.SetParent(p);
            entityList.Add(e);
            
        }

        public void Render(Matrix4 projMatrix, Shader shader, Texture cubemap, Vector3 c)
        {
            a += 0.01f;
            if (a > 2 * PI) a -= 2 * PI;
            entityList[2].Rotate(new Vector3(a, 0, 0));
            entityList[0].Rotate(new Vector3(0, a, 0));

            foreach (Entity m in entityList)
            {
                Matrix4 resultMatrix = m.ModelMatrix;

                Entity branch = m;    //follows the hierarchy
                while (branch.Parent != null)    //while it has a parent
                {
                    branch = branch.Parent;
                    resultMatrix *= branch.ModelMatrix;
                }
                if (m is EntitySkyReflect)
                    (m as EntitySkyReflect).Render(projMatrix, resultMatrix, c);
                else if (m is EntityFur)
                    (m as EntityFur).Render(projMatrix, resultMatrix);
            }



        }
    }
}