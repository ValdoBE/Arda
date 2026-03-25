using System.Numerics;
using Silk.NET.OpenGL;
using Arda.Renderer;

namespace Arda.Renderer.OpenGL;

public class OpenGLShader : IShader
{
    private readonly GL _gl;
    private readonly uint _handle;

    public OpenGLShader(GL gl, string vertexSrc, string fragmentSrc)
    {
        _gl = gl;

        uint vs = Compile(ShaderType.VertexShader, vertexSrc);
        uint fs = Compile(ShaderType.FragmentShader, fragmentSrc);

        _handle = _gl.CreateProgram();
        _gl.AttachShader(_handle, vs);
        _gl.AttachShader(_handle, fs);
        _gl.LinkProgram(_handle);

        _gl.GetProgram(_handle, GLEnum.LinkStatus, out int status);
        if (status == 0)
            throw new Exception($"Shader link error: {_gl.GetProgramInfoLog(_handle)}");

        _gl.DetachShader(_handle, vs);
        _gl.DetachShader(_handle, fs);
        _gl.DeleteShader(vs);
        _gl.DeleteShader(fs);
    }

    private uint Compile(ShaderType type, string src)
    {
        uint shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, src);
        _gl.CompileShader(shader);

        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
        if (status == 0)
            throw new Exception($"Shader compile error ({type}):\n{_gl.GetShaderInfoLog(shader)}");

        return shader;
    }

    public void Bind()   => _gl.UseProgram(_handle);
    public void Unbind() => _gl.UseProgram(0);

    public void SetInt(string name, int value)
        => _gl.Uniform1(Location(name), value);

    public void SetFloat(string name, float value)
        => _gl.Uniform1(Location(name), value);

    public void SetVector3(string name, Vector3 v)
        => _gl.Uniform3(Location(name), v.X, v.Y, v.Z);

    public void SetVector4(string name, Vector4 v)
        => _gl.Uniform4(Location(name), v.X, v.Y, v.Z, v.W);

    public unsafe void SetMatrix4(string name, Matrix4x4 value)
    {
        // System.Numerics row-major memory layout matches GLSL column-major — no transpose needed.
        _gl.UniformMatrix4(Location(name), 1, false, (float*)&value);
    }

    private int Location(string name) => _gl.GetUniformLocation(_handle, name);

    public void Dispose() => _gl.DeleteProgram(_handle);
}
