using NUnit.Framework;
using RenderCommon.BufferObject;

namespace RenderCommon.Test;

[TestFixture]
public class Tests
{
    private const float Tolerance = 1e-6f;

    [Test]
    public void PropertiesTest()
    {
        var rectangle = new Rect();
        Assert.That(new Rect(0f, 0f, 0f, 0f), Is.EqualTo(rectangle));

        rectangle = new Rect(0.1f, 0.3f, 0.5f, 0.7f);
        Assert.That(rectangle.Width, Is.EqualTo(0.5f));
        Assert.That(rectangle.Height, Is.EqualTo(0.7f));

        Assert.That(rectangle.Center, Is.EqualTo(new Point2D(0.1f, 0.3f)));
        Assert.That(rectangle.Size, Is.EqualTo(new Point2D(0.5f, 0.7f)));

        Assert.That(rectangle.Left, Is.EqualTo(-0.15f).Within(Tolerance));
        Assert.That(rectangle.Right, Is.EqualTo(0.35f).Within(Tolerance));
        Assert.That(rectangle.Bottom, Is.EqualTo(-0.05f).Within(Tolerance));
        Assert.That(rectangle.Top, Is.EqualTo(0.65f).Within(Tolerance));
    }

    [Test]
    public void FromBoundsTest()
    {
        var rectangle = Rect.FromBounds(0f, 5f, 10f, 20f);
        Assert.That(rectangle.Center.X, Is.EqualTo(5f).Within(Tolerance));
        Assert.That(rectangle.Center.Y, Is.EqualTo(12.5f).Within(Tolerance));
        
        Assert.That(rectangle.Width, Is.EqualTo(10f).Within(Tolerance));
        Assert.That(rectangle.Height, Is.EqualTo(15f).Within(Tolerance));
    }

    [Test]
    public void UnmanagedBufferTest() 
    {
        float centerX = 12.34f;
        float centerY = -50.5678f;
        float width = 505.12345f;
        float height = 22222.22222f;

        var center = new Point2D(centerX, centerY);
        var size = new Point2D(width, height);

        var rectangle = new Rect(center, size);
        Assert.That(rectangle.SizeOf(), Is.EqualTo(sizeof(float) * 4));

        var combinedBytes = center.AsByteArray().Concat(size.AsByteArray());
        CollectionAssert.AreEqual(combinedBytes, rectangle.AsByteArray());
    }
}