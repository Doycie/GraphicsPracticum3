using OpenTK;
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

        public void Render(Matrix4 projMatrix, Shader shader, Texture wood)
        {
            a = 0.01f;
            if (a > 2 * PI) a -= 2 * PI;
            entityList[2].ModelMatrix *= Matrix4.CreateRotationX(a);
            entityList[1].ModelMatrix *= Matrix4.CreateRotationY(a);
            entityList[0].ModelMatrix *= Matrix4.CreateRotationY(a);
            entityList[3].ModelMatrix *= Matrix4.CreateRotationX(-a);

           
            foreach (Entity m in entityList)
            {
                Matrix4 resultMatrix = m.ModelMatrix;

                Entity branch = m;    //follows the hierarchy
                while (branch.parent != null)    //while it has a parent
                {
                    branch = branch.parent;
                    resultMatrix *= branch.ModelMatrix;
                }

                m.mesh.Render(shader, projMatrix, resultMatrix, wood);
            }


        }
    }
}