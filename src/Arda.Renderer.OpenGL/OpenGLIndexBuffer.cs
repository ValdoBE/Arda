using Silk.NET.OpenGL;
using Arda.Renderer;

namespace Arda.Renderer.OpenGL;

public class OpenGLIndexBuffer : IIndexBuffer
{
    private readonly GL _gl;
    private readonly uint _handle;

    public uint Count { get; }

    public OpenGLIndexBuffer(GL gl, uint[] indices)
    {
        _gl = gl;
        Count = (uint)indices.Length;
        _handle = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _handle);
        _gl.BufferData(BufferTargetARB.ElementArrayBuffer,
            (nuint)(indices.Length * sizeof(uint)),
            indices.AsSpan(),
            BufferUsageARB.StaticDraw);
    }

    public void Bind()   => _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _handle);
    public void Unbind() => _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

    public void Dispose() => _gl.DeleteBuffer(_handle);
}
