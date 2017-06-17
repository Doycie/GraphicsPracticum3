using OpenTK;
using Template_P3;

public class EntitySkyReflect : Entity
{
    protected Texture skyboxtex;

    public EntitySkyReflect(Mesh m, Shader s, Texture tex, Texture sky) : base(m, s, tex)
    {
        skyboxtex = sky;
    }

    public override void Render(Camera c, Matrix4 m)
    {
        mesh.RenderSkyReflect(shader, c.getCameraMatrix(), m, texture, skyboxtex, c.camPos);
    }
}