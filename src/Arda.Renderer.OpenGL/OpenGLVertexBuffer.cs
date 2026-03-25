using Silk.NET.OpenGL;
using Arda.Renderer;

namespace Arda.Renderer.OpenGL;

public class OpenGLVertexBuffer : IVertexBuffer
{
    private readonly GL _gl;
    private readonly uint _handle;

    public BufferLayout Layout { get; set; } = null!;

    public OpenGLVertexBuffer(GL gl, float[] vertices)
    {
        _gl = gl;
        _handle = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _handle);
        _gl.BufferData(BufferTargetARB.ArrayBuffer,
            (nuint)(vertices.Length * sizeof(float)),
            vertices.AsSpan(),
            BufferUsageARB.StaticDraw);
    }

    public void Bind()   => _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _handle);
    public void Unbind() => _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

    public void Dispose() => _gl.DeleteBuffer(_handle);
}
