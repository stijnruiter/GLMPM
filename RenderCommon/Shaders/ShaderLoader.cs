using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace RenderCommon.Shaders;

public class ShaderLoader : IDisposable
{
    private int _handle;

    public ShaderLoader(string vertexPath, string fragmentPath)
    {
        if (!Compile(ShaderType.VertexShader, vertexPath, out int vertexShader))
            throw new Exception("Unable to compile vertex shader.");
        if (!Compile(ShaderType.FragmentShader, fragmentPath, out int fragShader))
            throw new Exception("Unable to compile frag shader.");
        if (!CreateProgram(vertexShader, fragShader))
            throw new Exception("Unable to create shader program.");
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    private bool CreateProgram(params int[] shaders)
    {
        _handle = GL.CreateProgram();
        foreach (var shader in shaders)
        {
            GL.AttachShader(_handle, shader);
        }

        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_handle);
            Console.WriteLine(infoLog);
            return false;
        }

        foreach (var shader in shaders)
        {
            GL.DetachShader(_handle, shader);
            GL.DeleteShader(shader);
        }

        return true;
    }

    private bool Compile(ShaderType type, string shaderFile, out int program)
    {
        program = -1;
        string shaderSource = File.ReadAllText(shaderFile);
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, shaderSource);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            Console.WriteLine(infoLog);
            return false;
        }

        program = shader;
        return true;
    }

    public int GetAttribLocation(string attribName)
    {
        var loc = GL.GetAttribLocation(_handle, attribName);
        if (loc < 0)
            throw new Exception($"Unable to get location of Attrib {attribName}");
        return loc;
    }

    public int GetUniformLocation(string uniformName)
    {
        var loc = GL.GetUniformLocation(_handle, uniformName);

        if (loc < 0)
            throw new Exception($"Unable to get location of Attrib {uniformName}");

        return loc;
    }

    public void SetUniform(int location, float value) => GL.Uniform1(location, value);
    public void SetUniform(int location, Vector2 value) => GL.Uniform2(location, value);
    public void SetUniform(int location, float v1, float v2) => GL.Uniform2(location, v1, v2);
    public void SetUniform(int location, Vector3 value) => GL.Uniform3(location, value);
    public void SetUniform(int location, float v1, float v2, float v3) => GL.Uniform3(location, v1, v2, v3);
    public void SetUniform(int location, Vector4 value) => GL.Uniform4(location, value);
    public void SetUniform(int location, float v1, float v2, float v3, float v4) => GL.Uniform4(location, v1, v2, v3, v4);
    public void SetUniform(int location, Matrix4 value) => GL.UniformMatrix4(location, false, ref value);

    public void SetUniform(string locationName, float value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, Vector2 value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, float v1, float v2) => SetUniform(GetUniformLocation(locationName), v1, v2);
    public void SetUniform(string locationName, Vector3 value) => SetUniform(GetUniformLocation(locationName), value);
    public void SetUniform(string locationName, float v1, float v2, float v3) => SetUniform(GetUniformLocation(locationName), v1, v2, v3);
    public void SetUniform(string locationName, Vector4 value) => SetUniform(GetUniformLocation(locationName), value);
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
            Console.WriteLine("Shader not disposed.");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
