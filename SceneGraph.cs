using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Template_P3
{
    class SceneGraph
    {
       
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
       

        public void Render(Matrix4 CameraMatrix, Shader shader, Texture wood)
        {

            foreach(Mesh m in meshList)
            {
                Matrix4 resultMatrix = m.ModelMatrix ;

                

                Mesh branch = m;    //follows the hierarchy
                while (branch.parent != null)    //while it has a parent
                {
                    
                    
                    branch = branch.parent;
                    resultMatrix *= branch.ModelMatrix ;

                }

                resultMatrix *= CameraMatrix;

                m.Render(shader, resultMatrix, wood);
            }
        }
    }
}
