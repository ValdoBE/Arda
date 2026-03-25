using Silk.NET.OpenGL;
using Arda.Renderer;

namespace Arda.Renderer.OpenGL;

public class OpenGLVertexArray : IVertexArray
{
    private readonly GL _gl;
    private readonly uint _handle;

    public IIndexBuffer? IndexBuffer { get; private set; }

    public OpenGLVertexArray(GL gl)
    {
        _gl = gl;
        _handle = _gl.GenVertexArray();
    }

    public void Bind()   => _gl.BindVertexArray(_handle);
    public void Unbind() => _gl.BindVertexArray(0);

    public unsafe void AddVertexBuffer(IVertexBuffer buffer)
    {
        _gl.BindVertexArray(_handle);
        buffer.Bind();

        var layout = buffer.Layout
            ?? throw new InvalidOperationException("Vertex buffer has no layout set.");

        uint attribIndex = 0;
        foreach (var element in layout.Elements)
        {
            _gl.EnableVertexAttribArray(attribIndex);
            _gl.VertexAttribPointer(
                attribIndex,
                element.ComponentCount,
                VertexAttribPointerType.Float,
                element.Normalized,
                layout.Stride,
                (void*)element.Offset);
            attribIndex++;
        }
    }

    public void SetIndexBuffer(IIndexBuffer buffer)
    {
        _gl.BindVertexArray(_handle);
        buffer.Bind();
        IndexBuffer = buffer;
    }

    public void Dispose() => _gl.DeleteVertexArray(_handle);
}
