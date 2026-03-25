namespace Arda.Renderer;

public interface IVertexArray : IDisposable
{
    IIndexBuffer? IndexBuffer { get; }
    void Bind();
    void Unbind();
    void AddVertexBuffer(IVertexBuffer buffer);
    void SetIndexBuffer(IIndexBuffer buffer);
}
