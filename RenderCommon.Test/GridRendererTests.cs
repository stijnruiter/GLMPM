using NUnit.Framework;
using RenderCommon.Test.Utils;
using System.Drawing;

namespace RenderCommon.Test;

[TestFixture]
internal class GridRendererTests : OpenGLTests
{
    protected override Color BackgroundColor => Color.White;
    protected override int Width => 100;
    protected override int Height => 100;

    [Test]
    public void RenderGrid()
    {
        var camera = new Camera()
        {
            ViewDomain = new BufferObject.Rect(2.01f, 2.01f, 4f, 4f),
            ViewportSize = (Width, Height),
            EqualAspectRatio = true,
        };
        using var render = new GridRenderer(camera.ViewDomain);
        render.Draw(camera.Projection, Color.Black);

        var output = CreateBitmap();
        BitmapAssert.AreEqual(output, GridBaseline);
    }

    private SparseMatrix<Color> GridBaseline
    {
        get
        {
            var baseline = new SparseMatrix<Color>(Width, Height, BackgroundColor);
            baseline[24, ..] = baseline[49, ..] = baseline[74, ..] = baseline[99, ..] = Color.Black;
            baseline[..,  0] = baseline[.., 25] = baseline[.., 50] = baseline[.., 75] = Color.Black;
            return baseline;
        }
    }
}
