using OpenTK;
using OpenTK.Input;

public class Camera
{
    //Location of the camera.
    public Vector3 camPos;

    //Angles.x is the angle of rotation around the z axis, angles.y is the rotation relative to the xy plane.
    public Vector2 rotation;

    //The vector to the center of the "plane" in front of the camera relative to the camera.
    public Vector3 Direction
    {
        get
        {
            Vector3 directionVector = new Vector3(RotationMatrix * new Vector4(0, 0, 1, 0));
            return directionVector;
        }
    }

    //Returns a normalized vector pointing to the left from the camera's point of view.
    private Vector3 DLeft
    {
        get
        {
            Vector3 directionVector = new Vector3(RotationMatrix * new Vector4(1, 0, 0, 0));
            return directionVector;
        }
    }

    //Returns a normalized vector pointing to down from the camera's point of view.
    private Vector3 DDown
    {
        get
        {
            Vector3 directionVector = new Vector3(RotationMatrix * new Vector4(0, 1, 0, 0));
            return directionVector;
        }
    }

    public Matrix4 RotationMatrix
    {
        get
        {
            Matrix4 matrix = Matrix4.CreateRotationY(rotation.Y);
            return matrix * Matrix4.CreateRotationX(rotation.X);
        }
    }

    public Matrix4 CameraMatrix
    {
        get
        {
            Matrix4 transform = Matrix4.CreateTranslation(camPos);
            transform *= RotationMatrix;
            transform *= CameraProjMatrix;
            return transform;
        }
    }

    public Matrix4 CameraRotationMatrix
    {
        get
        {
            Matrix4 matrix = Matrix4.CreateRotationY(rotation.Y);
            return matrix * Matrix4.CreateRotationX(rotation.X);
        }
    }

    public Matrix4 CameraModelMatrix
    {
        get
        {
            Matrix4 transform = Matrix4.CreateTranslation(camPos);
            transform *= RotationMatrix;
            return transform;
        }
    }

    public Matrix4 CameraProjMatrix
    {
        get
        {
            Matrix4 transform = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
            return transform;
        }
    }

    //translates and rotates the camera based on the input
    public void Input(KeyboardState k)
    {
        if (k.IsKeyDown(OpenTK.Input.Key.W))
        {
            camPos += Direction;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.S))
        {
            camPos -= Direction;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.A))
        {
            camPos += DLeft;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.D))
        {
            camPos -= DLeft;
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

        if (k.IsKeyDown(OpenTK.Input.Key.Q))
        {
            camPos += DDown;
        }
        if (k.IsKeyDown(OpenTK.Input.Key.E))
        {
            camPos -= DDown;
        }
    }
}