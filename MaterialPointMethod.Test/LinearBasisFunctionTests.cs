using NUnit.Framework;

namespace MaterialPointMethod.Test;

[TestFixture]
internal class LinearBasisFunctionTests
{
    private const float Tolerance = 1e-6f;

    [Test]
    public void LinearShapeFunctionConstructor()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new LinearBasis([0f]));
        Assert.Throws<ArgumentException>(() => new LinearBasis([0f, 1f, 2f, 1f]));
    }

    [Test]
    public void SampleSimpleShapeFunction()
    {
        var nodes = new float[] { 0f, 1f, 2f, 3f, 4f };
        var shapeFunction = new LinearBasis(nodes);

        Assert.That(shapeFunction.Sample(0.0f)[0], Is.EqualTo(1.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(0.5f)[0], Is.EqualTo(0.5f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(1.0f)[0], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(1.5f)[0], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(2.0f)[0], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(3.0f)[0], Is.EqualTo(0.0f).Within(Tolerance));

        Assert.That(shapeFunction.Sample(0.0f)[1], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(0.5f)[1], Is.EqualTo(0.5f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(1.0f)[1], Is.EqualTo(1.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(1.5f)[1], Is.EqualTo(0.5f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(2.0f)[1], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(3.0f)[1], Is.EqualTo(0.0f).Within(Tolerance));

        Assert.That(shapeFunction.Sample(0.0f)[2], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(0.5f)[2], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(1.0f)[2], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(1.5f)[2], Is.EqualTo(0.5f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(2.0f)[2], Is.EqualTo(1.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(3.0f)[2], Is.EqualTo(0.0f).Within(Tolerance));

        Assert.That(shapeFunction.Sample(0.0f)[4], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(2.0f)[4], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(2.5f)[4], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(3.0f)[4], Is.EqualTo(0.0f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(3.5f)[4], Is.EqualTo(0.5f).Within(Tolerance));
        Assert.That(shapeFunction.Sample(4.0f)[4], Is.EqualTo(1.0f).Within(Tolerance));
    }

    [Test]
    public void SampleSimpleShapeFunctionException()
    {
        var nodes = new float[] { 0f, 1f, 2f, 3f, 4f };
        var shapeFunction = new LinearBasis(nodes);
        Assert.Throws<ArgumentOutOfRangeException>(() => shapeFunction.Sample(-1f));
        Assert.Throws<ArgumentOutOfRangeException>(() => shapeFunction.Sample(4.1f));
    }
}
