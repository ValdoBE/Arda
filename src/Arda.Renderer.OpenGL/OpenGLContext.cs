using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Arda.Renderer;

namespace Arda.Renderer.OpenGL;

public class OpenGLContext : IGraphicsContext
{
    private GL? _gl;
    private readonly IWindow _nativeWindow;

    public GL GL => _gl ?? throw new InvalidOperationException("Context not initialized.");

    public OpenGLContext(IWindow nativeWindow)
    {
        _nativeWindow = nativeWindow;
    }

    public void Initialize()
    {
        _gl = GL.GetApi(_nativeWindow);
        _gl.Enable(EnableCap.DepthTest);
    }

    public void Clear(float r, float g, float b, float a = 1f)
    {
        GL.ClearColor(r, g, b, a);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void Dispose() => _gl?.Dispose();
}
