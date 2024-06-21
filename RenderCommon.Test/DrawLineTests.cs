using NUnit.Framework;
using OpenTK.Mathematics;
using RenderCommon.BufferObject;
using RenderCommon.Test.Utils;
using System.Drawing;

namespace RenderCommon.Test;

[TestFixture]
internal class DrawLineTests : OpenGLTests
{
    protected override Color BackgroundColor => Color.White;
    protected override int Width => 400;
    protected override int Height => 400;

    private Camera _camera;
    
    [SetUp]
    public void CreateCamear()
    {
        _camera = new Camera
        {
            ViewDomain = Rect.FromBounds(1, 1, Width, Height),
            ViewportSize = (Width, Height),
            EqualAspectRatio = true,
        };
    }


    [Test]
    public void DrawLine()
    {
        using var renderer = new LineRenderer();
        var points = new Point2D[]
        {
            new Point2D(100, 100),
            new Point2D(300, 100),
            new Point2D(300, 300),
            new Point2D(100, 300),
            new Point2D(100, 100),
        };

        renderer.Draw(_camera.Projection, points);
        var output = CreateBitmap();
        BitmapAssert.AreEqual(output, DrawLineBox);
    }

    [Test]
    public void DrawLineSegments()
    {
        using var renderer = new LineRenderer();
        var points = new Line2D[]
        {
            new Line2D(new Point2D(100, 100), new Point2D(100,  300)),
            new Line2D(new Point2D(100, 300), new Point2D(300,  300)),
            new Line2D(new Point2D(300, 300), new Point2D(300, 100)),
            new Line2D(new Point2D(300, 100), new Point2D(100, 100)),
        };

        renderer.Draw(_camera.Projection, points);
        var output = CreateBitmap();
        BitmapAssert.AreEqual(output, DrawLineBox);
    }

    private SparseMatrix<Color> DrawLineBox
    {
        get
        {
            var sparseMat = new SparseMatrix<Color>(Width, Height, Color.White);
            sparseMat[99..299, 100] = Color.Black;
            sparseMat[99..299, 300] = Color.Black;
            sparseMat[99, 100..300] = Color.Black;
            sparseMat[299, 100..300] = Color.Black;
            return sparseMat;
        }
    }
}
