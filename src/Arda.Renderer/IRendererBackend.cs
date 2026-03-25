namespace Arda.Renderer;

public interface IRendererBackend
{
    IVertexBuffer CreateVertexBuffer(float[] vertices);
    IIndexBuffer CreateIndexBuffer(uint[] indices);
    IVertexArray CreateVertexArray();
    IShader CreateShader(string vertexSrc, string fragmentSrc);
}
