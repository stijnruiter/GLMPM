using NUnit.Framework;
using RenderCommon.Shaders;
using RenderCommon.Test.Utils;
using System.Diagnostics.Metrics;

namespace RenderCommon.Test;

[TestFixture]
internal class ShaderLoaderTests : OpenGLTests
{
    private const string DefaultFragmentShaderSource = "#version 330 core\r\nuniform vec4 color;out vec4 FragColor;void main() { FragColor = color; }";
    private const string DefaultVertexShaderSource = "#version 330 core\r\nin vec2 vertices;uniform mat4 model;uniform mat4 projection;void main() { gl_Position = projection * model * vec4(vertices, 0.0, 1.0); }";

    [Test]
    public void LoadException()
    {
        var exception = Assert.Throws<ShaderException>(() => new ShaderLoader(DefaultVertexShaderSource, DefaultVertexShaderSource));
        Assert.That(exception.Message, Does.StartWith("Unable to compile shader"));

        exception = Assert.Throws<ShaderException>(() => new ShaderLoader(string.Empty, string.Empty));
        Assert.That(exception.Message, Does.StartWith("Unable to link shaders"));

        var correctShader = new ShaderLoader(DefaultVertexShaderSource, DefaultFragmentShaderSource);

        exception = Assert.Throws<ShaderException>(() => correctShader.GetAttribLocation("non_existent_location"));
        Assert.That(exception.Message, Is.EqualTo("Unable to get location of Attrib non_existent_location"));

        exception = Assert.Throws<ShaderException>(() => correctShader.SetUniform("non_existent_location", 5f));
        Assert.That(exception.Message, Is.EqualTo("Unable to get location of Uniform non_existent_location"));
    }
}
