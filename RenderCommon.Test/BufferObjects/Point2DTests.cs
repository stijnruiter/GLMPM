using NUnit.Framework;
using RenderCommon.BufferObject;
using RenderCommon.Test.Utils;

namespace RenderCommon.Test.BufferObjects;

[TestFixture]
public class Point2DTests
{
    [Test]
    public void PropertiesTest()
    {
        var p1 = new Point2D();

        Assert.That(p1, Is.EqualTo(new Point2D(0f, 0f)));

        p1.X = 0.1f;
        p1.Y = 0.3f;

        var p2 = new Point2D(0.1f, 0.3f);

        Assert.That(p2, Is.EqualTo(p1));
    }

    [Test]
    public void UnmanagedBufferTest()
    {
        float x = -22222.1234f;
        float y = 12.3f;

        var p1 = new Point2D(x, y);
        Assert.That(p1.SizeOf(), Is.EqualTo(sizeof(float) * 2));

        var combinedArray = x.AsByteArray().Concat(y.AsByteArray());

        CollectionAssert.AreEqual(p1.AsByteArray(), combinedArray);
    }
}
