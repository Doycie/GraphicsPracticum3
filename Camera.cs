using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


public class Camera
{
    private Vector3 CamPos = new Vector3(0, 0, 15);
    private Vector3 viewdirection = new Vector3(0, 0, -1);
    private Vector3 rightdirection = new Vector3(1, 0, 0);
    private Vector3 updirection = new Vector3(0, 1, 0);

    public Matrix4 getCameraMatrix()
    {
        Matrix4 transform = Matrix4.CreateTranslation(CamPos);
        transform *= new Matrix4(new Vector4(rightdirection, 0), new Vector4(updirection, 0), new Vector4(viewdirection, 0), new Vector4(0, 0, 0, 1));
        transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
        return transform;
    }

    public void RotateAroundX(float angle)
    {
        viewdirection = viewdirection * (float)Math.Cos(angle) + updirection * (float)Math.Sin(angle);
        viewdirection.Normalize();
        updirection = -Vector3.Cross(viewdirection, rightdirection);
    }

    public void RotateAroundY(float angle)
    {
        viewdirection = viewdirection * (float)Math.Cos(angle) - rightdirection * (float)Math.Sin(angle);
        viewdirection.Normalize();
        rightdirection = Vector3.Cross(viewdirection, updirection);
    }

    public void RotateAroundZ(float angle)
    {
        rightdirection = rightdirection * (float)Math.Cos(angle) + updirection * (float)Math.Sin(angle);
        rightdirection.Normalize();
        updirection = -Vector3.Cross(viewdirection, rightdirection);
    }

    public void Input(OpenTK.Input.KeyboardState k)
    {
        if (k.IsKeyDown(OpenTK.Input.Key.W))
        {
            CamPos += viewdirection;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.S))
        {
            CamPos -= viewdirection;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.A))
        {
            CamPos += rightdirection;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.D))
        {
            CamPos -= rightdirection;
        }

        if (k.IsKeyDown(OpenTK.Input.Key.Up))
        {
            RotateAroundX(-0.1f);
        }
        if (k.IsKeyDown(OpenTK.Input.Key.Down))
        {
            RotateAroundX(0.1f);
        }

        if (k.IsKeyDown(OpenTK.Input.Key.Right))
        {
            RotateAroundY(0.1f);
        }
        if (k.IsKeyDown(OpenTK.Input.Key.Left))
        {
            RotateAroundY(-0.1f);
        }

        if (k.IsKeyDown(OpenTK.Input.Key.Q))
        {
            RotateAroundZ(0.1f);
        }
        if (k.IsKeyDown(OpenTK.Input.Key.E))
        {
            RotateAroundZ(-0.1f);
        }
    }

}

