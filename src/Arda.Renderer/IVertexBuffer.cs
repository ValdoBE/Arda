namespace Arda.Renderer;

public interface IVertexBuffer : IDisposable
{
    BufferLayout Layout { get; set; }
    void Bind();
    void Unbind();
}
