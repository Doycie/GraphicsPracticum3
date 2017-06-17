using OpenTK;
using System.Collections.Generic;
using Template_P3;

public class Entity
{
    public Vector3 rotation;
    public Vector3 translation;
    public Vector3 scale;
    protected Mesh mesh;

    protected Shader shader;

    public List<Entity> children;         //points to the parent, if it stays null it does not have a parent
    protected Texture texture;       // texture to use for rendering

    public Entity(Mesh m, Shader s, Texture tex)
    {
        texture = tex;
        mesh = m;
        shader = s;
        scale = new Vector3(1, 1, 1);
        children = new List<Entity>();
    }

    public Matrix4 ModelMatrix
    {
        get
        {
            Matrix4 modelMatrix = Utility.CreateRotationXYZ(rotation);
            modelMatrix *= Matrix4.CreateTranslation(translation);
            modelMatrix *= Matrix4.CreateScale(scale);
            return modelMatrix;
        }
    }

    public void Rotate(Vector3 v)
    {
        rotation = v;
    }

    public void AddChild(Entity e)
    {
        children.Add(e);
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

    public virtual void Render(Camera c, Matrix4 m)
    {
        mesh.Render(shader, c.getCameraMatrix(), m, texture);
    }
}