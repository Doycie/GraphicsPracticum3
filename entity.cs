using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template_P3;
using OpenTK;

public class Entity
{

    public Mesh mesh;
    public Matrix4 ModelMatrix; //Matrix for the modelview  
    public Entity parent;         //points to the parent, if it stays null it does not have a parent

    public Entity(Mesh m)
    {
        mesh = m;
        ModelMatrix = new Matrix4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
      }
    public void SetParent(Entity e) {
        parent = e;
    }

    public void SetPostition(Vector3 v)
    {
        ModelMatrix = Matrix4.CreateTranslation(v);
    }
    public void Move(Vector3 v)
    {
        ModelMatrix *= Matrix4.CreateTranslation(v);
    }
    public void Scale(Vector3 v)
    {
        ModelMatrix *= Matrix4.CreateScale(v);
    }


}

