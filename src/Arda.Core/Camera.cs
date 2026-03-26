using System.Numerics;
using Arda.ECS;

namespace Arda.Core;

public class Camera : Component
{
    public Matrix4x4 ViewMatrix = Matrix4x4.Identity;
    public Matrix4x4 ProjectionMatrix = Matrix4x4.Identity;
    public Vector3 Position;
}
