namespace Arda.Renderer;

public static class RendererFactory
{
    private static IRendererBackend? _backend;

    public static RendererAPI API { get; private set; } = RendererAPI.None;

    public static void RegisterBackend(IRendererBackend backend, RendererAPI api)
    {
        _backend = backend;
        API = api;
    }

    public static IVertexBuffer CreateVertexBuffer(float[] vertices)
        => Backend().CreateVertexBuffer(vertices);

    public static IIndexBuffer CreateIndexBuffer(uint[] indices)
        => Backend().CreateIndexBuffer(indices);

    public static IVertexArray CreateVertexArray()
        => Backend().CreateVertexArray();

    public static IShader CreateShader(string vertexSrc, string fragmentSrc)
        => Backend().CreateShader(vertexSrc, fragmentSrc);

    private static IRendererBackend Backend()
        => _backend ?? throw new InvalidOperationException(
               "No renderer backend registered. Call RendererFactory.RegisterBackend() first.");
}
