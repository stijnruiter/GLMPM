namespace RenderCommon.Shaders;

internal class ShaderException : Exception
{
    public ShaderException(string message) : base(message) { }

    public ShaderException(string message, string info) : base($"{message} {info}") { }
}
