using System.Numerics;
using Arda.ECS.Core;
using Arda.Renderer;

namespace Arda.Core.Components;

public class MeshRenderer : Component
{
    public IVertexArray VertexArray = null!;
    public uint VertexCount;
    public IShader Shader = null!;
    public Vector3 ObjectColor;
}
