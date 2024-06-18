using NUnit.Framework;
using OpenTK.Mathematics;
using RenderCommon.BufferObject;
using System.Drawing;

namespace RenderCommon.Test;

[TestFixture]
internal class DrawLineTests : OpenGLTests
{
    protected override Color BackgroundColor => Color.White; 

    [Test]
    public void DrawLine()
    {
        var projection = Matrix4.Identity;
        using var renderer = new LineRenderer();
        var points = new Point2D[]
        {
            new Point2D(-0.5f, -0.5f),
            new Point2D(-0.5f,  0.5f),
            new Point2D( 0.5f,  0.5f),
            new Point2D( 0.5f, -0.5f),
            new Point2D(-0.5f, -0.5f),
        };

        renderer.Draw(projection, points);

        var bitmap = CreateBitmap();
        bitmap.Save("DrawLineBox.png", System.Drawing.Imaging.ImageFormat.Png);
        BitmapAssert.AreEqual(bitmap, "DrawLineBox.png");
    }

    [Test]
    public void DrawLineSegments()
    {
        var projection = Matrix4.Identity;
        using var renderer = new LineRenderer();
        var points = new Line2D[]
        {
            new Line2D(new Point2D(-0.5f, -0.5f), new Point2D(-0.5f,  0.5f)),
            new Line2D(new Point2D( 0.5f,  0.5f), new Point2D( 0.5f, -0.5f)),
            new Line2D(new Point2D(0.0f,  -1.0f), new Point2D(1.0f, 1.0f))
        };

        renderer.Draw(projection, points);

        var bitmap = CreateBitmap();
        bitmap.Save("DrawLineSegmentBox.png", System.Drawing.Imaging.ImageFormat.Png);
        BitmapAssert.AreEqual(bitmap, "DrawLineSegmentBox.png");
    }
}
