namespace Arda.Renderer;

public interface IIndexBuffer : IDisposable
{
    uint Count { get; }
    void Bind();
    void Unbind();
}
