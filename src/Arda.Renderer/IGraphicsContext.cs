namespace Arda.Renderer;

public interface IGraphicsContext : IDisposable
{
    void Initialize();
    void Clear(float r, float g, float b, float a = 1f);
}
