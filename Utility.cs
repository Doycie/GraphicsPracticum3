using OpenTK;

namespace Template_P3
{
    class Utility
    {
        public static Matrix4 CreateRotationXYZ(Vector3 rotation)
        {
            Matrix4 matrix = Matrix4.CreateRotationX(rotation.X);
            matrix *= Matrix4.CreateRotationY(rotation.Y);
            return Matrix4.CreateRotationZ(rotation.Z) * matrix;
        }
    }
}