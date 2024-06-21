using NUnit.Framework;
using OpenTK.Mathematics;
using RenderCommon.BufferObject;
using RenderCommon.Test.Utils;
using System.Drawing;

namespace RenderCommon.Test;

[TestFixture]
internal class RectangleRendererTests : OpenGLTests
{
    protected override Color BackgroundColor => Color.Black; 
    protected override int Width => 200;
    protected override int Height => 200;

    [Test]
    public void RenderRectangle()
    {
        var rect = Rect.FromBounds(50, 50, 150, 150);
        using var render = new RectangleRenderer(rect);
        var camera = new Camera()
        {
            ViewDomain = Rect.FromBounds(1, 1, Width, Height),
            ViewportSize = (Width, Height),
            EqualAspectRatio = true,
        };
        render.Draw(camera.Projection, Color4.White);
        var output = CreateBitmap();
        BitmapAssert.AreEqual(output, RectangleBaseline);
    }

    private SparseMatrix<Color> RectangleBaseline
    {
        get
        {
            var baseline = new SparseMatrix<Color>(Width, Height, BackgroundColor);
            baseline[49..149, 49..149] = Color.White;
            return baseline;
        }
    }
}
