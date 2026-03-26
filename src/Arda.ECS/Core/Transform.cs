using System.Numerics;

namespace Arda.ECS.Core;

public class Transform : Component
{
    public Vector3 Position = Vector3.Zero;
    public Vector3 Rotation = Vector3.Zero;  // Euler angles in degrees
    public Vector3 Scale = Vector3.One;

    public Matrix4x4 LocalToWorldMatrix =>
        Matrix4x4.CreateScale(Scale) *
        Matrix4x4.CreateRotationY(Rotation.Y * MathF.PI / 180f) *
        Matrix4x4.CreateRotationX(Rotation.X * MathF.PI / 180f) *
        Matrix4x4.CreateRotationZ(Rotation.Z * MathF.PI / 180f) *
        Matrix4x4.CreateTranslation(Position);
}
