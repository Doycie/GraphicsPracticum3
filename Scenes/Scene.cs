using System.Collections.Generic;
using System.Diagnostics;
using Template_P3;

namespace template_P3
{
    abstract class Scene
    {
        protected SceneGraph scenegraph;

        List<Entity> Lights;

        public Scene()
        {
        }


        public abstract void Load();

        public abstract void Update(long delta_t);

        public abstract void Draw(Camera c);

        public abstract void DrawPost(ScreenQuad q, RenderTarget t);
    }
}
