using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RenderCommon.BufferObject;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[DebuggerDisplay("Start ({Start.X}, {Start.Y}), End ({End.X}, {End.Y})")]
public struct Line2D : IBoundingBox
{
    public Point2D Start;
    public Point2D End;

    public Rect BoundingBox => BufferObjectFunctions.GetBoundingBox(Start, End);

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