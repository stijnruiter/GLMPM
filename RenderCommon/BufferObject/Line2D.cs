using System.Runtime.InteropServices;

namespace RenderCommon.BufferObject;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Line2D
{
    public Point2D Start;
    public Point2D End;

    public Line2D(float startx, float starty, float endx, float endy)
        : this(new Point2D(startx, starty), new Point2D(endx, endy))
    {
    }

    public Line2D(Point2D start, Point2D end)
    {
        Start = start;
        End = end;
    }
}