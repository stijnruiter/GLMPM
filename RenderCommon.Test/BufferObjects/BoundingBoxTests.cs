using NUnit.Framework;
using NUnit.Framework.Constraints;
using RenderCommon.BufferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderCommon.Test.BufferObjects;

[TestFixture]
internal class BoundingBoxTests
{
    [Test]
    public void GetBoundingBoxFromPoints()
    {
        var p1 = new Point2D(0.1f, 5.5f);
        var p2 = new Point2D(-1.1f, 1.3f);
        var p3 = new Point2D(3.3f, -2.2f);

        Assert.That(BufferObjectFunctions.GetBoundingBox(p1), Is.EqualTo(new Rect(0.1f, 5.5f, 0f, 0f)));
        Assert.That(BufferObjectFunctions.GetBoundingBox(p1, p2), Is.EqualTo(Rect.FromBounds(-1.1f, 1.3f, 0.1f, 5.5f)).Within(1e-6));
        Assert.That(BufferObjectFunctions.GetBoundingBox(p1, p2, p3), Is.EqualTo(Rect.FromBounds(-1.1f, -2.2f, 3.3f, 5.5f)).Within(1e-6));
    }

    [Test]
    public void GetBoundingBoxFromBoundingBoxes()
    {
        var r1 = new Rect(0.1f, 5.5f, 1, 5);
        var r2 = new Rect(-1.1f, 1.3f, 4, 2);
        var r3 = new Rect(5.3f, 3.2f, 1, 1);


        AssertAreEquals(BufferObjectFunctions.GetBoundingBox(r1), new Rect(0.1f, 5.5f, 1, 5));
        var rect = BufferObjectFunctions.GetBoundingBox(r1, r2);

        AssertAreEquals(rect, Rect.FromBounds(-3.1f, 0.3f, 0.9f, 8f));
        rect = BufferObjectFunctions.GetBoundingBox(r1, r2, r3);
        AssertAreEquals(rect, Rect.FromBounds(-3.1f, 0.3f, 5.8f, 8f));
    }

    [Test]
    public void ParticleBoundingBox()
    {
        var particle = new Particle2D();
        Assert.That(particle.BoundingBox, Is.EqualTo(new Rect()));

        particle = new Particle2D(2.3f, 3.3f, 5.5f);
        Assert.That(particle.BoundingBox, Is.EqualTo(new Rect(2.3f, 3.3f, 5.5f, 5.5f)));
    }

    [Test]
    public void Line2DBoundingBox()
    {
        var line = new Line2D();
        Assert.That(line.BoundingBox, Is.EqualTo(new Rect()));

        line = new Line2D(2.3f, 3.3f, 5.5f, 2f);
        Assert.That(line.BoundingBox, Is.EqualTo(new Rect(3.9f, 2.65f, 3.2f, 1.3f)));
    }

    private void AssertAreEquals(Rect actual, Rect expected, float tolerance = 1e-6f)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.Center.X, Is.EqualTo(expected.Center.X).Within(tolerance));
            Assert.That(actual.Center.Y, Is.EqualTo(expected.Center.Y).Within(tolerance));
            Assert.That(actual.Size.X, Is.EqualTo(expected.Size.X).Within(tolerance));
            Assert.That(actual.Size.Y, Is.EqualTo(expected.Size.Y).Within(tolerance));
        });
    }
}
