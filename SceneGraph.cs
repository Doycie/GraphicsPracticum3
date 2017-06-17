using OpenTK;
using System;
using System.Collections.Generic;

namespace Template_P3
{
    public class SceneGraph
    {
        const float PI = 3.1415926535f;
        float a = 0;
        public List<Entity> rootEntities;

        public SceneGraph()
        {
            rootEntities = new List<Entity>();
        }

        public void AddRootEntity(Entity e)
        {
            rootEntities.Add(e);
        }

        public void AddEntity(Entity e, Entity p)
        {
            p.AddChild(e);
        }

        public void Render(Camera c)
        {
            for(int i = 0; i < rootEntities.Count; i++)
            {
                RenderEntity(rootEntities[i], c, Matrix4.Identity);
            }
        }

        private void RenderEntity(Entity e, Camera c, Matrix4 m)
        {
            Matrix4 res = e.ModelMatrix * m;
            e.Render(c, res);
            for(int i = 0; i < e.children.Count; i++)
                RenderEntity(e.children[i], c, res);
        }
    }
}