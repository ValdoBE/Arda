using System.Numerics;
using Arda.ECS;
using Arda.Renderer;

namespace Arda.Core;

public class MeshRenderer : Component
{
    public IVertexArray VertexArray = null!;
    public uint VertexCount;
    public IShader Shader = null!;
    public Vector3 ObjectColor;
}
