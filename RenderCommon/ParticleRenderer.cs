using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RenderCommon.Shaders;
using System.Runtime.InteropServices;

namespace RenderCommon
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Particle2D
    {
        public float X;
        public float Y;
        public float Size;

        public Particle2D(float x, float y, float size)
        {
            X = x;
            Y = y;
            Size = size;
        }
    }

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

        public void Draw(Matrix4 viewMatrix, Particle2D[] data) 
        {
            _particleShader.Use();
            _particleShader.SetUniform("view", viewMatrix);

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

}
