using NUnit.Framework;
using RenderCommon.BufferObject;
using RenderCommon.Test.Utils;
using System.Drawing;

namespace RenderCommon.Test;

[TestFixture]
internal class ParticleRendererTests : OpenGLTests
{
    protected override Color BackgroundColor => Color.White;
    protected override int Width => 100;
    protected override int Height => 100;

    [Test]
    public void RenderParticle()
    {
        Window.IsVisible = true;
        var camera = new Camera()
        {
            ViewDomain = Rect.FromBounds(0, 0, 20, 20),
            ViewportSize = (Width, Height),
            EqualAspectRatio = true,
        };
        using var renderer = new ParticleRenderer();
        var particles = new Particle2D[]
        {
            new Particle2D( 5,  5, 1),
            new Particle2D( 5, 15, 3),
            new Particle2D(15, 15, 2),
            new Particle2D(15,  5, 1),
        };
        renderer.Draw(camera.Projection, camera.ViewportSize, Color.Blue, particles);

        var output = CreateBitmap();
        BitmapAssert.AreEqual(output, ParticleBaseline);
    }

    private SparseMatrix<Color> ParticleBaseline
    {
        get
        {
            var baseline = new SparseMatrix<Color>(Width, Height, BackgroundColor);
            // new Particle2D( 5,  5, 1), new Particle2D(15,  5, 1)
            baseline[23..26, 73..76] = baseline[73..76, 73..76] = Color.Blue;

            //new Particle2D(15, 15, 2),
            baseline[70..79, 23..26] = baseline[71..78, 21..28] = baseline[73..76, 20..29] = Color.Blue;

            //new Particle2D(5, 15, 3),
            baseline[18..31, 21..28] = baseline[19..30, 20..29] = baseline[20..29, 19..30] 
                = baseline[21..28, 18..31] = Color.Blue;

            return baseline;
        }
    }
}
