using Silk.NET.OpenGL;
using Arda.Renderer;

namespace Arda.Renderer.OpenGL;

public class OpenGLRendererBackend : IRendererBackend
{
    private readonly GL _gl;

    public OpenGLRendererBackend(GL gl)
    {
        _gl = gl;
    }

    public IVertexBuffer CreateVertexBuffer(float[] vertices)
        => new OpenGLVertexBuffer(_gl, vertices);

    public IIndexBuffer CreateIndexBuffer(uint[] indices)
        => new OpenGLIndexBuffer(_gl, indices);

    public IVertexArray CreateVertexArray()
        => new OpenGLVertexArray(_gl);

    public IShader CreateShader(string vertexSrc, string fragmentSrc)
        => new OpenGLShader(_gl, vertexSrc, fragmentSrc);
}
