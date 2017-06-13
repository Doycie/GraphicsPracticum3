using OpenTK;
using System.Collections.Generic;

namespace Template_P3
{
    public class SceneGraph
    {
        const float PI = 3.1415926535f;
        float a = 0;
        public List<Mesh> meshList;

        public SceneGraph()
        {
            meshList = new List<Mesh>();
        }

        public void AddMesh(Mesh m, Mesh p)
        {
            m.parent = p;
            meshList.Add(m);
        }

        public void Render(Matrix4 projMatrix, Shader shader, Texture wood)
        {
            foreach (Mesh m in meshList)
            {
                Matrix4 resultMatrix = m.ModelMatrix;

                Mesh branch = m;    //follows the hierarchy
                while (branch.parent != null)    //while it has a parent
                {
                    branch = branch.parent;
                    resultMatrix *= branch.ModelMatrix;
                }

                m.Render(shader, projMatrix, resultMatrix, wood);
            }
            a = 0.01f;
            if (a > 2 * PI) a -= 2 * PI;
            meshList[2].ModelMatrix *= Matrix4.CreateRotationX(a);
            meshList[1].ModelMatrix *= Matrix4.CreateRotationY(a);
            meshList[0].ModelMatrix *= Matrix4.CreateRotationY(a);
            meshList[3].ModelMatrix *= Matrix4.CreateRotationX(-a);
           

        }
    }
}