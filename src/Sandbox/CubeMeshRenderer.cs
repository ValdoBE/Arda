using System.Numerics;
using Arda.ECS;
using Arda.Renderer;
using Arda.Renderer.OpenGL;

class CubeMeshRenderer : MonoBehaviour
{
    public IVertexArray VertexArray = null!;
    public uint VertexCount;
    public IShader Shader = null!;
    public Vector3 ObjectColor;
    public Vector3 CameraPos;

    private OpenGLContext _ctx = null!;

    public void Setup(OpenGLContext ctx, IVertexArray va, uint vertexCount, IShader shader)
    {
        _ctx = ctx;
        VertexArray = va;
        VertexCount = vertexCount;
        Shader = shader;
    }

    public override void Render(float dt)
    {
        var model = Transform.LocalToWorldMatrix;
        var view  = Matrix4x4.CreateLookAt(CameraPos, Vector3.Zero, Vector3.UnitY);
        var proj  = Matrix4x4.CreatePerspectiveFieldOfView(
                        MathF.PI / 4f, 1280f / 720f, 0.1f, 100f);

        Shader.Bind();
        Shader.SetMatrix4("uModel",       model);
        Shader.SetMatrix4("uView",        view);
        Shader.SetMatrix4("uProjection",  proj);
        Shader.SetVector3("uLightPos",    new Vector3(2f, 3f, 2f));
        Shader.SetVector3("uLightColor",  Vector3.One);
        Shader.SetVector3("uObjectColor", ObjectColor);
        Shader.SetVector3("uViewPos",     CameraPos);

        VertexArray.Bind();
        _ctx.GL.DrawArrays(Silk.NET.OpenGL.PrimitiveType.Triangles, 0, VertexCount);
    }

    public override void OnDestroy()
    {
        VertexArray?.Dispose();
        Shader?.Dispose();
    }
}
