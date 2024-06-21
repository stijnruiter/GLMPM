using NUnit.Framework;
using RenderCommon.Shaders;
using RenderCommon.Test.Utils;

namespace RenderCommon.Test;

[TestFixture]
internal class ShaderLoaderTests : OpenGLTests
{
    private const string DefaultFragmentShaderSource = "#version 330 core\r\nin vec4 color;out vec4 FragColor;void main() { FragColor = color; }";
    private const string DefaultVertexShaderSource = "#version 330 core\r\nin vec2 vertices; out vec4 color;void main() { gl_Position = vec4(vertices, 0.0, 1.0); }";

    private const string LinkFailVertexShaderSource = "#version 330 core\r\n void main() { }";
    private const string LinkFailFragmentShaderSource = "#version 330 core\r\n void main2() { }";

    [Test]
    public void LoadException()
    {
        var exception = Assert.Throws<ShaderException>(() => new ShaderLoader(DefaultVertexShaderSource, DefaultVertexShaderSource));
        Assert.That(exception.Message, Does.StartWith("Unable to compile shader"));

        exception = Assert.Throws<ShaderException>(() => new ShaderLoader(LinkFailVertexShaderSource, LinkFailFragmentShaderSource));
        Assert.That(exception.Message, Does.StartWith("Unable to link shaders"));

        var correctShader = new ShaderLoader(DefaultVertexShaderSource, DefaultFragmentShaderSource);

        exception = Assert.Throws<ShaderException>(() => correctShader.GetAttribLocation("non_existent_location"));
        Assert.That(exception.Message, Is.EqualTo("Unable to get location of Attrib non_existent_location"));

        exception = Assert.Throws<ShaderException>(() => correctShader.SetUniform("non_existent_location", 5f));
        Assert.That(exception.Message, Is.EqualTo("Unable to get location of Uniform non_existent_location"));
    }
}
