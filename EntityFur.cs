using OpenTK;
using Template_P3;

public class EntityFur : Entity
{
    private const int furmatrices = 5;
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

    public override void Render(Camera c, Matrix4 m)
    {
        a += 0.1f;

        for (int i = 0; i < furmatrices; i++)
        {
            int j = matrixcounter - i - 1;
            if (j < 0)
            {
                j += furmatrices;
            }
            mesh.RenderFur(shader, c.getCameraMatrix(), ma[j], texture, i * 0.04f);
        }
        ma[matrixcounter] = m;
        matrixcounter++;
        if (matrixcounter >= furmatrices)
        {
            matrixcounter = 0;
        }
    }
}