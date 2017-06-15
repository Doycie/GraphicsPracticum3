using OpenTK;
using Template_P3;

public class Entity
{
    protected Vector3 rotation;
    protected Vector3 translation;
    protected Vector3 scale;
    protected Mesh mesh;

    protected Shader shader;

    protected Entity parent;         //points to the parent, if it stays null it does not have a parent
    protected Texture texture;                           // texture to use for rendering

    public Entity(Mesh m, Shader s, Texture tex)
    {
        texture = tex;
        mesh = m;
        shader = s;
        scale = new Vector3(1, 1, 1);
    }
    public Entity Parent
    {
        get { return parent; }
    }
    public Matrix4 ModelMatrix
    {
        get
        {
            Matrix4 modelMatrix = Utility.CreateRotationXYZ(rotation);
            modelMatrix *= Matrix4.CreateScale(scale);
            return Matrix4.CreateTranslation(translation) * modelMatrix;
        }
    }
    public void Rotate( Vector3 v){
        rotation = v;
        
        }
    public void SetParent(Entity e)
    {
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