namespace RenderCommon.BufferObject;

internal static class BufferObjectFunctions
{
    public static Rect GetBoundingBox(params Point2D[] points)
        => Rect.FromBounds(points.Min(p => p.X), points.Min(p => p.Y), points.Max(p => p.X), points.Max(p => p.Y));

    public static Rect GetBoundingBox(params IBoundingBox[] boundingBoxes)
        => Rect.FromBounds(
            boundingBoxes.Min(r => r.BoundingBox.Left),
            boundingBoxes.Min(r => r.BoundingBox.Bottom),
            boundingBoxes.Max(r => r.BoundingBox.Right),
            boundingBoxes.Max(r => r.BoundingBox.Top));
}
