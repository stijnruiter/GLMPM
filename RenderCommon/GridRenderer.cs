using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RenderCommon.Shaders;
using System.Runtime.InteropServices;

namespace RenderCommon;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Point
{
	public float X;
	public float Y;

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Line
{
	public Point Start;
	public Point End;

	public Line(float startx, float starty,  float endx, float endy) 
		: this(new Point(startx, starty), new Point(endx, endy))
	{ 
	}

	public Line(Point start, Point end)
	{
		Start = start;
		End = end;
	}
}

class GridRenderer : IDisposable
{
    private ShaderLoader _shader;
    private VertexObject _vao;
    private VertexBuffer _vbo;

    private Line[] _lines;

    public GridRenderer(Rect domain, int step = 1)
	{
		_lines = CreateHorizontalVerticalLines(domain, step);

		_shader = new ShaderLoader("Shaders/Vertex/default.vert", "Shaders/Fragment/default.frag");
        _shader.Use();
		_shader.SetUniform("model", Matrix4.Identity);

		_vao = new VertexObject();
        _vao.Bind();

        _vbo = new VertexBuffer(BufferTarget.ArrayBuffer);
		_vbo.SetData(_lines);
        _vbo.SetVertexAttributeFloat(_shader.GetAttribLocation("vertices"), 2);
	}

    public void Draw(Matrix4 projection)
    {
        _shader.Use();
		_shader.SetUniform("projection", projection);
        _vao.Bind();
        GL.DrawArrays(PrimitiveType.Lines, 0, _lines.Length * 2);
	}

	public void Dispose()
	{
        _shader.Dispose();
        _vao.Dispose();
        _vbo.Dispose();
	}

    private Line[] CreateHorizontalVerticalLines(Rect domain, int step)
    {
		var lines = new List<Line>();
		int start = (int)domain.Left;
		while (start <= domain.Right)
		{
			lines.Add(new Line(start, domain.Bottom, start, domain.Top));
			start += step;
		}
		start= (int)domain.Bottom;
		while (start <= domain.Top)
		{
			lines.Add(new Line(domain.Left, start, domain.Right, start));
			start += step;
		}

		return lines.ToArray();
	}
}