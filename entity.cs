using OpenTK;
using Template_P3;

public class Entity
{
    public Vector3 rotation;
    public Vector3 translation;
    public Vector3 scale;
    public Mesh mesh;
    public Matrix4 ModelMatrix
    {
        get
        {
            Matrix4 modelMatrix = Utility.CreateRotationXYZ(rotation);
            modelMatrix *= Matrix4.CreateScale(scale);
            return Matrix4.CreateTranslation(translation) * modelMatrix;
        }
    }
    public Entity parent;         //points to the parent, if it stays null it does not have a parent
    public Texture texture;                           // texture to use for rendering

    public Entity(Mesh m, Texture tex)
    {
        texture = tex;
        mesh = m;
        scale = new Vector3(1, 1, 1);
    }
    public void SetParent(Entity e) {
        parent = e;
    }

    public void SetPostition(Vector3 v)
    {
        translation = v;
    }
    public void Move(Vector3 v)
    {
        translation += v;
    }
    public void Scale(Vector3 v)
    {
        scale = v;
    }
}

