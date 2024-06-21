using NUnit.Framework;
using RenderCommon.BufferObject;
using RenderCommon.Test.Utils;

namespace RenderCommon.Test.BufferObjects;

[TestFixture]
public class Line2DTests
{
    [Test]
    public void PropertiesTest()
    {
        var line = new Line2D();
        Assert.That(line, Is.EqualTo(new Line2D(0f, 0f, 0f, 0f)));

        var start = new Point2D(-22.22222f, 33.1234f);
        var end = new Point2D(12345.6789f, -0.012345f);
        line = new Line2D(start, end);

        Assert.That(line.Start, Is.EqualTo(start));
        Assert.That(line.End, Is.EqualTo(end));
    }

    [Test]
    public void UnmanagedBufferTest()
    {
        var start = new Point2D(-22.22222f, 33.1234f);
        var end = new Point2D(12345.6789f, -0.012345f);
        var line = new Line2D(start, end);

        Assert.That(line.SizeOf(), Is.EqualTo(sizeof(float) * 4));

        var combinedArray = start.AsByteArray().Concat(end.AsByteArray());

        CollectionAssert.AreEqual(line.AsByteArray(), combinedArray);
    }
}
