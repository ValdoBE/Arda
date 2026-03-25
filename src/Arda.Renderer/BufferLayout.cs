namespace Arda.Renderer;

public class BufferElement
{
    public string Name { get; }
    public ShaderDataType Type { get; }
    public uint Size { get; }
    public uint Offset { get; set; }
    public bool Normalized { get; }

    public int ComponentCount => Type.ComponentCount();

    public BufferElement(string name, ShaderDataType type, bool normalized = false)
    {
        Name = name;
        Type = type;
        Size = type.SizeOf();
        Normalized = normalized;
    }
}

public class BufferLayout
{
    private readonly List<BufferElement> _elements;

    public IReadOnlyList<BufferElement> Elements => _elements;
    public uint Stride { get; private set; }

    public BufferLayout(IEnumerable<BufferElement> elements)
    {
        _elements = [..elements];
        CalculateOffsetsAndStride();
    }

    private void CalculateOffsetsAndStride()
    {
        uint offset = 0;
        foreach (var element in _elements)
        {
            element.Offset = offset;
            offset += element.Size;
        }
        Stride = offset;
    }
}
