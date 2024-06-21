using NUnit.Framework;
using RenderCommon.BufferObject;
using RenderCommon.Test.Utils;

namespace RenderCommon.Test.BufferObjects;

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

        Assert.Throws<ArgumentOutOfRangeException>(() => rectangle.Height = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => rectangle.Width = -1);
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

    [Test]
    public void MorphologyTest()
    {
        var rect = new Rect();
        rect.Dilate(2f);
        Assert.That(rect, Is.EqualTo(new Rect(0f, 0f, 4f, 4f)));

        rect.Dilate(1f);
        Assert.That(rect, Is.EqualTo(new Rect(0f, 0f, 6f, 6f)));

        rect.Erode(2f);
        Assert.That(rect, Is.EqualTo(new Rect(0f, 0f, 2f, 2f)));

        var rect1 = new Rect(2, 2, 5, 5);
        var rect2 = new Rect(2, 2, 5, 5);

        rect1.Dilate(2);
        rect2.Erode(-2);

        Assert.That(rect1, Is.EqualTo(rect2));

        rect1.Erode(1);
        rect2.Dilate(-1);
        Assert.That(rect1, Is.EqualTo(rect2));

        Assert.Throws<ArgumentOutOfRangeException>(() => rect1.Erode(5));
    }
}