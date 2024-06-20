using System.Runtime.InteropServices;

namespace RenderCommon.BufferObject;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Rect : IBoundingBox
{
    public Point2D Center;
    public Point2D Size;

    public float Left => Center.X - Size.X / 2f;
    public float Right => Center.X + Size.X / 2f;
    public float Bottom => Center.Y - Size.Y / 2f;
    public float Top => Center.Y + Size.Y / 2f;

    public float Width
    {
        get => Size.X;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            Size.X = value;
        }
    }

    public float Height
    {
        get => Size.Y;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value); 
            Size.Y = value;
        }
    }

    public Rect BoundingBox => this;

    public Rect(float cx, float cy, float width, float height) : this(new Point2D(cx, cy), new Point2D(width, height))
    {
    }

    public Rect(Point2D center, Point2D size)
    {
        Center = center;
        Size = size;
    }

    public static Rect FromBounds(float left, float bottom, float right, float top)
        => new Rect((left + right) / 2, (top + bottom) / 2, right - left, top - bottom);

    public void Dilate(float kernel)
    {
        Size.X += 2 * kernel;
        Height += 2 * kernel;
    }

    public void Erode(float kernel)
    {
        Size.X -= 2 * kernel;
        Height -= 2 * kernel;
    }
}
