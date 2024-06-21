using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace RenderCommon.Shaders;

public class ShaderLoader : IDisposable
{
    public ShaderLoader(string vertexSource, string fragmentSource)
    {
        var vertexShader = Compile(ShaderType.VertexShader, vertexSource);
        var fragShader = Compile(ShaderType.FragmentShader, fragmentSource);

        _handle = LinkAndDisposeShaders(vertexShader, fragShader);
    }

    public static ShaderLoader FromFiles(string vertexPath, string fragmentPath)
    {
        var vertexSource = File.ReadAllText(vertexPath);
        var fragmentSource = File.ReadAllText(fragmentPath);
        return new ShaderLoader(vertexSource, fragmentSource);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    private int LinkAndDisposeShaders(params int[] shaders)
    {
        var handle = GL.CreateProgram();
        foreach (var shader in shaders)
        {
            GL.AttachShader(handle, shader);
        }

        GL.LinkProgram(handle);

        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
            throw new ShaderException("Unable to link shaders.", GL.GetProgramInfoLog(handle));

        foreach (var shader in shaders)
        {
            GL.DetachShader(_handle, shader);
            GL.DeleteShader(shader);
        }
        return handle;
    }

    private int Compile(ShaderType type, string shaderSource)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, shaderSource);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
            throw new ShaderException($"Unable to compile shader of type '{type}'", GL.GetShaderInfoLog(shader));

        return shader;
    }

    public int GetAttribLocation(string attribName)
    {
        var loc = GL.GetAttribLocation(_handle, attribName);
        if (loc < 0)
            throw new ShaderException($"Unable to get location of Attrib {attribName}");
        return loc;
    }

    public int GetUniformLocation(string uniformName)
    {
        var loc = GL.GetUniformLocation(_handle, uniformName);

        if (loc < 0)
            throw new ShaderException($"Unable to get location of Uniform {uniformName}");

        return loc;
    }

    public void SetUniform(int location, float value) => GL.Uniform1(location, value);
    public void SetUniform(int location, Vector2 value) => GL.Uniform2(location, value);
    public void SetUniform(int location, float v1, float v2) => GL.Uniform2(location, v1, v2);
    public void SetUniform(int location, Vector3 value) => GL.Uniform3(location, value);
    public void SetUniform(int location, float v1, float v2, float v3) => GL.Uniform3(location, v1, v2, v3);
    public void SetUniform(int location, Vector4 value) => GL.Uniform4(location, value);
    public void SetUniform(int location, Color4 value) => GL.Uniform4(location, value);
    public void SetUniform(int location, float v1, float v2, float v3, float v4) => GL.Uniform4(location, v1, v2, v3, v4);
    public void SetUniform(int location, Matrix4 value) => GL.UniformMatrix4(location, false, ref value);

    public void SetUniform(string locationName, float value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, Vector2 value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, float v1, float v2) => SetUniform(GetUniformLocation(locationName), v1, v2);
    public void SetUniform(string locationName, Vector3 value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, float v1, float v2, float v3) => SetUniform(GetUniformLocation(locationName), v1, v2, v3);
    public void SetUniform(string locationName, Vector4 value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, Color4 value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, float v1, float v2, float v3, float v4) => SetUniform(GetUniformLocation(locationName), v1, v2, v3, v4);
    public void SetUniform(string locationName, Matrix4 value) => SetUniform(GetUniformLocation(locationName), value);



    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(_handle);

            disposedValue = true;
        }
    }

    ~ShaderLoader()
    {
        if (disposedValue == false)
        {
            Debug.Fail("Shader not disposed");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public static ShaderLoader Default => ShaderLoader.FromFiles(VertexShaderDefaultPath, FragmentShaderDefaultPath);

    public static ShaderLoader Particle => ShaderLoader.FromFiles(VertexShaderParticlePath, FragmentShaderParticlePath);

    private const string VertexShaderDefaultPath = "Shaders/Vertex/default.vert";
    private const string VertexShaderParticlePath = "Shaders/Vertex/particle.vert";

    private const string FragmentShaderDefaultPath = "Shaders/Fragment/default.frag";
    private const string FragmentShaderParticlePath = "Shaders/Fragment/particle.frag";
    
    private readonly int _handle;
}
