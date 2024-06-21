using NUnit.Framework;
using OpenTK.Mathematics;

namespace RenderCommon.Test;

[TestFixture]
internal class CameraTests
{
    [Test]
    public void CreateOrthographicProjection()
    {
        Camera camera = new Camera();
        camera.ViewDomain = new BufferObject.Rect(2, 2, 1, 1);
        camera.ViewportSize = (100, 100);
        camera.EqualAspectRatio = false;

        var mat = Matrix4.CreateScale(2, 2, 1) * Matrix4.CreateTranslation(-4, -4, 0);

        Assert.That(camera.Projection, Is.EqualTo(mat));

        camera.EqualAspectRatio = true;
        Assert.That(camera.Projection, Is.EqualTo(mat));

        camera.ViewDomain = new BufferObject.Rect(2, 2, 2, 1);
        mat = Matrix4.CreateTranslation(-2f, -2.5f, 0);
        Assert.That(camera.Projection, Is.EqualTo(mat));

        camera.ViewportSize = (200, 50);
        mat = Matrix4.CreateScale(0.5f, 2, 1) * Matrix4.CreateTranslation(-1.5f, -4f, 0);
        Assert.That(camera.Projection, Is.EqualTo(mat).Within(1e-4));
    }
}
