using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Template_P3
{
    class SceneGraph
    {
        Matrix3 cameraMatrix; //based on user input
        List<Mesh> meshList;
        public SceneGraph()
        {
            meshList = new List<Mesh>();
        }
        public void Render()
        {
            foreach(Mesh m in meshList)
            {
                Matrix3 resultMatrix = m.viewMatrix;
                Mesh branch = m;    //follows the hierarchy
                while(branch.parent != null)    //while it has a parent
                {
                    branch = branch.parent;
                    //multiply resultMatrix and branch.viewMatrix
                }
            }
        }
    }
}
