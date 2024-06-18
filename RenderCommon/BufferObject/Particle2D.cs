using System.Runtime.InteropServices;

namespace RenderCommon.BufferObject;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct Particle2D
{
    public Point2D Position;
    public float Size;

    public Particle2D(float x, float y, float size) : this(new Point2D(x, y), size)
    {
    }

    public Particle2D(Point2D position, float size)
    {
        Position = position;
        Size = size;
    }
}
