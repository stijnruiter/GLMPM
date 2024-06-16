using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RenderCommon.Shaders;

namespace RenderCommon;

internal class RectangleRenderer : IDisposable
{
    private float[] _vertices =
    {
        -0.5f, -0.5f,
        -0.5f,  0.5f,
         0.5f, -0.5f,

         0.5f, -0.5f,
         0.5f,  0.5f,
        -0.5f,  0.5f
    };

    private ShaderLoader _shader;
    private VertexObject _vao;
    private VertexBuffer _vbo;

    public RectangleRenderer(Rect rect) 
    { 
        _shader = new ShaderLoader("Shaders/Vertex/default.vert", "Shaders/Fragment/default.frag");

        _vao = new VertexObject();
        _vao.Bind();

        _vbo = new VertexBuffer(BufferTarget.ArrayBuffer);
        _vbo.SetData(_vertices);
        _vbo.SetVertexAttributeFloat(_shader.GetAttribLocation("vertices"), 2);

        _shader.Use();
        var model = Matrix4.CreateScale(rect.Width, rect.Height, 0) * Matrix4.CreateTranslation(rect.CX, rect.CY, 0);
        _shader.SetUniform("color", 1.0f, 1.0f, 1.0f, 1.0f);
        _shader.SetUniform("model", model);
    }

    public void Dispose()
    {
        _shader.Dispose();
        _vao.Dispose();
        _vbo.Dispose();
    }

    public void Draw(Matrix4 view)
    {
        _shader.Use();
        _shader.SetUniform("view", view);
        _vao.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
}
