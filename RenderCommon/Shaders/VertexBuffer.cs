using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;

namespace RenderCommon.Shaders;

internal class VertexBuffer : IDisposable
{
    private int _handle;
    private BufferTarget _target;

    public VertexBuffer(BufferTarget target)
    {
        _handle = GL.GenBuffer();
        _target = target;
    }

    public void Bind()
    {
        GL.BindBuffer(_target, _handle);
    }

    public void SetData<T>(T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw) where T : struct
    {
        Bind();
        GL.BufferData(_target, data.Length * Unsafe.SizeOf<T>(), data, hint);
    }

    public void SetVertexAttribute(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
    {
        Bind();
        GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
        GL.EnableVertexAttribArray(index);
    }

    public void SetVertexAttributeFloat(int index, int count, int offset = 0)
    { 
        SetVertexAttribute(index, count, VertexAttribPointerType.Float, false, count * sizeof(float), offset);
    }

    public void Unbind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(_handle);
        Unbind();
        _handle = -1;
    }
}
