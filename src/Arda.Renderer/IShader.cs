using System.Numerics;

namespace Arda.Renderer;

public interface IShader : IDisposable
{
    void Bind();
    void Unbind();
    void SetInt(string name, int value);
    void SetFloat(string name, float value);
    void SetVector3(string name, Vector3 value);
    void SetVector4(string name, Vector4 value);
    void SetMatrix4(string name, Matrix4x4 value);
}
