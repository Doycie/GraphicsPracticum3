using System;
using OpenTK;


public class Camera
{
    //FOV in radiants
    float FOV;

    //Location of the camera.
    public Vector3 camPos;

    //Angles.x is the angle of rotation around the z axis, angles.y is the rotation relative to the xy plane.
    Vector2 rotation;

    //The vector to the center of the "plane" in front of the camera relative to the camera.
    Vector3 direction
    {
        get
        {
            Vector3 directionVector = new Vector3(rotationMatrix * new Vector4(0, 0, 1, 0));
            return directionVector;
        }
    }

    Vector3 dLeft
    {
        get
        {
            Vector3 directionVector = new Vector3(rotationMatrix * new Vector4(1, 0, 0, 0));
            return directionVector;
        }
    }

    Matrix4 rotationMatrix
    {
        get
        {
            Matrix4 matrix = Matrix4.CreateRotationY(rotation.Y);
            return matrix * Matrix4.CreateRotationX(rotation.X);
        }
    }

    public Matrix4 getCameraMatrix()
    {
        Matrix4 transform = Matrix4.CreateTranslation(camPos);
        transform *= rotationMatrix;
        transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
        return transform;
    }
    public Matrix4 getCameraRotationMatrix()
    {
        Matrix4 matrix = Matrix4.CreateRotationY(rotation.Y);
        return matrix * Matrix4.CreateRotationX(rotation.X);
    }
    public Matrix4 getCameraModelMatrix()
    {
        Matrix4 transform = Matrix4.CreateTranslation(camPos);
        transform *= rotationMatrix;
        return transform;
    }
    public Matrix4 getCameraProjMatrix()
    {
        Matrix4 transform = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
        return transform;
    }

    public void Input(OpenTK.Input.KeyboardState k)
    {
        if (k.IsKeyDown(OpenTK.Input.Key.W))
        {
            camPos += direction;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.S))
        {
            camPos -= direction;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.A))
        {
            camPos += dLeft;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.D))
        {
            camPos -= dLeft;
        }

        if (k.IsKeyDown(OpenTK.Input.Key.Up))
        {
            rotation.X -= 0.1f;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.Down))
        {
            rotation.X += 0.1f;
        }

        if (k.IsKeyDown(OpenTK.Input.Key.Right))
        {
            rotation.Y += 0.1f;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.Left))
        {
            rotation.Y -= 0.1f;
        }

 /*       if (k.IsKeyDown(OpenTK.Input.Key.Q))
        {
            RotateAroundZ(0.1f);
        }
        if (k.IsKeyDown(OpenTK.Input.Key.E))
        {
            RotateAroundZ(-0.1f);
        } */
    }
}

