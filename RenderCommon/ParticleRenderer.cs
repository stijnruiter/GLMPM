using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RenderCommon.BufferObject;
using RenderCommon.Shaders;

namespace RenderCommon;

internal class ParticleRenderer : IDisposable
{
    private ShaderLoader _particleShader;
    private VertexObject _vao;
    private VertexBuffer _particleVbo;
    private VertexBuffer _vertexVbo;

    private readonly float[] _vertices =
    {
        -0.5f, -0.5f,
        -0.5f,  0.5f,
         0.5f, -0.5f,

         0.5f, -0.5f,
         0.5f,  0.5f,
        -0.5f,  0.5f
    };

    public ParticleRenderer()
    {
        _particleShader = new ShaderLoader("Shaders/Vertex/particle.vert", "Shaders/Fragment/particle.frag");

        _vao = new VertexObject();
        _vao.Bind();

        _vertexVbo = new VertexBuffer(BufferTarget.ArrayBuffer);
        _vertexVbo.SetData(_vertices);
        _vertexVbo.SetVertexAttributeFloat(_particleShader.GetAttribLocation("vertices"), 2);

        _particleVbo = new VertexBuffer(BufferTarget.ArrayBuffer);
        _particleVbo.Bind();
        _particleVbo.SetVertexAttributeFloat(_particleShader.GetAttribLocation("particle"), 3);
        GL.VertexAttribDivisor(_particleShader.GetAttribLocation("particle"), 1);
    }

    public void Draw(Matrix4 projection, Vector2 viewportSize, Particle2D[] data) 
    {
        _particleShader.Use();
        _particleShader.SetUniform("projection", projection);
        _particleShader.SetUniform("resolution", viewportSize);
        _vao.Bind();
        _particleVbo.SetData(data, BufferUsageHint.StreamDraw);
        GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, 6, data.Length);
    }

    public void Dispose() 
    {
        _vertexVbo.Dispose();
        _particleVbo.Dispose();
        _vao.Dispose();
        _particleShader.Dispose();
    }
}
