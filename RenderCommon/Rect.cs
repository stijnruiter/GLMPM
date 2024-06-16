using System.Runtime.InteropServices;

namespace RenderCommon;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Rect
{
    public float CX;
    public float CY;

    public float Width;
    public float Height;

    public Rect(float cx, float cy, float width, float height)
    {
        CX = cx;
        CY = cy;
        Width = width;
        Height = height;
    }

    public static Rect FromBounds(float left, float top, float right, float bottom) 
        => new Rect((left + right) / 2, (top + bottom) / 2, right - left, top - bottom);

    public void Expand(float kernel)
    {
        Width += 2 * kernel;
        Height += 2 * kernel;
    }
}
