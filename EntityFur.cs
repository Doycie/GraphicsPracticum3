using OpenTK;
using System;
using Template_P3;

public class EntityFur : Entity
{
    private const int furmatrices = 20;
    private float a = 0;
    private Matrix4[] ma = new Matrix4[furmatrices];

    private int matrixcounter = 0;

    public EntityFur(Mesh m, Shader s, Texture tex) : base(m, s, tex)
    {
        for (int i = 0; i < furmatrices; i++)
        {

            ma[i] = (Matrix4.Identity);
        }

    }

    public void Render(Matrix4 proj,Matrix4 m)
    {
      
        a += 0.1f;

        for (int i = 0; i < furmatrices; i++)
        {
            int j = i + matrixcounter;
            if (j >= furmatrices)
            {
                j -= furmatrices;
            }
            mesh.RenderFur(shader, proj, ma[j], texture, i * 0.04f);
        }
        ma[matrixcounter] = m;
        matrixcounter++;
        if (matrixcounter >= furmatrices)
        {
            matrixcounter = 0;
        }
    }
}