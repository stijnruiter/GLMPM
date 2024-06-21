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
            ViewDomain = new BufferObject.Rect(0, 0, 4, 4),
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
            baseline[.., 24] = baseline[.., 49] = baseline[.., 74] = baseline[.., 99] = Color.Black;
            return baseline;
        }
    }
}
