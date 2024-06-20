using NUnit.Framework;
using OpenTK.Mathematics;
using RenderCommon.BufferObject;
using System.Drawing;

namespace RenderCommon.Test;

[TestFixture]
internal class DrawLineTests : OpenGLTests
{
    protected override Color BackgroundColor => Color.White;
    protected override int Width => 400;
    protected override int Height => 400;


    [Test]
    public void DrawLine()
    {
        var projection = Matrix4.CreateOrthographicOffCenter(1, 400, 1, 400, -1, 1);
        using var renderer = new LineRenderer();
        var points = new Point2D[]
        {
            new Point2D(100, 100),
            new Point2D(300, 100),
            new Point2D(300, 300),
            new Point2D(100, 300),
            new Point2D(100, 100),
        };

        renderer.Draw(projection, points);
        var bitmap = CreateBitmap();
        Assert.That((bitmap.Width, bitmap.Height), Is.EqualTo((Width, Height)), "Bitmap dimensions mismatch.");
        BitmapAssert.AreEqual(bitmap, DrawLineBox);
    }

    [Test]
    public void DrawLineSegments()
    {
        var projection = Matrix4.CreateOrthographicOffCenter(1, 400, 1, 400, -1, 1);
        using var renderer = new LineRenderer();
        var points = new Line2D[]
        {
            new Line2D(new Point2D(100, 100), new Point2D(100,  300)),
            new Line2D(new Point2D(100, 300), new Point2D(300,  300)),
            new Line2D(new Point2D(300, 300), new Point2D(300, 100)),
            new Line2D(new Point2D(300, 100), new Point2D(100, 100)),
        };

        renderer.Draw(projection, points);
        var bitmap = CreateBitmap();
        Assert.That((bitmap.Width, bitmap.Height), Is.EqualTo((Width, Height)), "Bitmap dimensions mismatch.");
        BitmapAssert.AreEqual(bitmap, DrawLineBox);
    }

    private SparseMatrix<Color> DrawLineBox
    {
        get
        {
            var sparseMat = new SparseMatrix<Color>(Width, Height, Color.White);
            sparseMat[99..299, 99] = Color.Black;
            sparseMat[99..299, 299] = Color.Black;
            sparseMat[99, 99..299] = Color.Black;
            sparseMat[299, 99..299] = Color.Black;
            return sparseMat;
        }
    }
}
