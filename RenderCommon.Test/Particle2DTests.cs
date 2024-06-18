using NUnit.Framework;
using RenderCommon.BufferObject;

namespace RenderCommon.Test;

[TestFixture]
public class Particle2DTests
{
    [Test]
    public void PropertiesTest()
    {
        var particle = new Particle2D();
        Assert.That(particle, Is.EqualTo(new Particle2D(0f, 0f, 0f)));

        particle = new Particle2D(-5f, 10f, 1.1f);
        Assert.That(particle.Position, Is.EqualTo(new Point2D(-5f, 10f)));
        Assert.That(particle.Size, Is.EqualTo(1.1f));
    }

    [Test]
    public void UnmanagedBufferTest()
    {
        var position = new Point2D(-5.5555f, 10.1234f);
        var size = 0.01f;
        var particle = new Particle2D(position.X, position.Y, size);

        Assert.That(particle.SizeOf(), Is.EqualTo(sizeof(float) * 3));

        var combinedArray = position.AsByteArray().Concat(size.AsByteArray());
        CollectionAssert.AreEqual(particle.AsByteArray(), combinedArray);
    }
}
