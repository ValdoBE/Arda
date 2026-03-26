using System.Numerics;
using Arda.ECS.Core;

namespace Arda.Core.Components;

public class Camera : Component
{
    public Matrix4x4 ViewMatrix = Matrix4x4.Identity;
    public Matrix4x4 ProjectionMatrix = Matrix4x4.Identity;
    public Vector3 Position;
}
