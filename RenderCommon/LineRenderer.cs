using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RenderCommon.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderCommon
{
    internal class LineRenderer
    {
        private ShaderLoader ShaderLoader { get; }
        private VertexObject VertexObject { get; }
        private VertexBuffer VertexBuffer { get; }
        private VertexBuffer ElementBuffer { get; }

        public LineRenderer() {
            ShaderLoader = new ShaderLoader("Shaders/Vertex/default.vert", "Shaders/Fragment/default.frag");
            
            VertexObject = new VertexObject();
            VertexObject.Bind();

            VertexBuffer = new VertexBuffer(BufferTarget.ArrayBuffer);
            VertexBuffer.Bind();
            VertexBuffer.SetVertexAttributeFloat(ShaderLoader.GetAttribLocation("vertices"), 2);

            ElementBuffer = new VertexBuffer(BufferTarget.ElementArrayBuffer);
        }

        public void Draw(Matrix4 projection, Point[] points)
        {
            ShaderLoader.Use();
            ShaderLoader.SetUniform("model", Matrix4.Identity);
            ShaderLoader.SetUniform("projection", projection);
            ShaderLoader.SetUniform("color", new Vector4(0f, 0f, 0f, 1f));

            VertexObject.Bind();
            VertexBuffer.SetData(points);

            if (points.Length != _nPoints)
            {
                _nPoints = points.Length;
                _elements = GenIndices(points.Length - 1);
                ElementBuffer.SetData(_elements);
            }

            GL.DrawElements(PrimitiveType.Lines, _elements.Length, DrawElementsType.UnsignedInt, 0);
        }
        public void Draw(Matrix4 projection, Line[] lines)
        {
            ShaderLoader.Use();
            ShaderLoader.SetUniform("model", Matrix4.Identity);
            ShaderLoader.SetUniform("projection", projection);
            ShaderLoader.SetUniform("color", new Vector4(0f, 0f, 0f, 1f));

            VertexObject.Bind();
            VertexBuffer.SetData(lines);

            GL.DrawArrays(PrimitiveType.Lines, 0, lines.Length * 2);
        }

        private uint[] _elements = new uint[0];
        private int _nPoints = 0;

        private uint[] GenIndices(int nElements)
        {
            var indices = new uint[nElements * 2];

            for (uint i = 0; i < nElements; i++)
            {
                indices[i*2] = i;
                indices[i * 2 + 1] = i + 1;
            }
            return indices;
        }
    }
}
