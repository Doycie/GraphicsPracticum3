using OpenTK;
using System.Collections.Generic;
using Template_P3;

public abstract class Entity
{
    public Vector3 rotation;
    public Vector3 translation;
    public Vector3 scale;

    protected Shader shader;
    protected Mesh mesh;
    protected Texture texture;       // texture to use for rendering

    public List<Entity> children;         
    public Entity parent = null;     //points to the parent, if it stays null it does not have a parent

    // Method loops through all the parents, slow for rendering but necesarry for eg. lights.
    public Vector3 GlobalLocation
    {
        get
        {
            if (parent == null)
                return translation;
            else
                return new Vector3(new Vector4(translation, 1.0f) * parent.GlobalModelMatrix);
        }
    }

    // Returns the matrix used to transform this entity to worldspace.
    public Matrix4 GlobalModelMatrix
    {
        get
        {
            if (parent == null)
                return ModelMatrix;
            else
                return ModelMatrix * parent.GlobalModelMatrix;
        }
    }

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
        e.parent = this;
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

    public abstract void Render(Camera c, Matrix4 m);
}