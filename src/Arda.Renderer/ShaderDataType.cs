namespace Arda.Renderer;

public enum ShaderDataType
{
    None,
    Float, Float2, Float3, Float4,
    Mat3, Mat4,
    Int, Int2, Int3, Int4,
    Bool
}

public static class ShaderDataTypeExtensions
{
    public static uint SizeOf(this ShaderDataType type) => type switch
    {
        ShaderDataType.Float  => 4,
        ShaderDataType.Float2 => 8,
        ShaderDataType.Float3 => 12,
        ShaderDataType.Float4 => 16,
        ShaderDataType.Mat3   => 36,
        ShaderDataType.Mat4   => 64,
        ShaderDataType.Int    => 4,
        ShaderDataType.Int2   => 8,
        ShaderDataType.Int3   => 12,
        ShaderDataType.Int4   => 16,
        ShaderDataType.Bool   => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    public static int ComponentCount(this ShaderDataType type) => type switch
    {
        ShaderDataType.Float  => 1,
        ShaderDataType.Float2 => 2,
        ShaderDataType.Float3 => 3,
        ShaderDataType.Float4 => 4,
        ShaderDataType.Mat3   => 9,
        ShaderDataType.Mat4   => 16,
        ShaderDataType.Int    => 1,
        ShaderDataType.Int2   => 2,
        ShaderDataType.Int3   => 3,
        ShaderDataType.Int4   => 4,
        ShaderDataType.Bool   => 1,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}
