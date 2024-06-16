using OpenTK.Graphics.OpenGL4;

namespace RenderCommon.Shaders;

internal class VertexObject : IDisposable
{
    private int _handle;

    public VertexObject()
    {
        _handle = GL.GenVertexArray();
    }

    public void Bind()
    {
        GL.BindVertexArray(_handle);
    }


    public void Unbind()
    {
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteVertexArrays(1, ref _handle);
        Unbind();
        _handle = -1;
    }
}
