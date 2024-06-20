using System.Runtime.InteropServices;

namespace RenderCommon.BufferObject;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Particle2D : IBoundingBox
{
    public Point2D Position;
    public float Size;

    public Rect BoundingBox => new Rect(Position, new Point2D(Size, Size));

    public Particle2D(float x, float y, float size) : this(new Point2D(x, y), size)
    {
    }

    public Particle2D(Point2D position, float size)
    {
        Position = position;
        Size = size;
    }
}
