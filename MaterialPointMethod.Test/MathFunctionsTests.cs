using NUnit.Framework;

namespace MaterialPointMethod.Test;

[TestFixture]
public class MathFunctionsTests
{
    [Test]
    public void LinSpace()
    {
        float[] values = MathFunctions.LinSpace(0f, 2f, 5);
        CollectionAssert.AreEqual(values, new float[] { 0f, 0.5f, 1f, 1.5f, 2f });

        values = MathFunctions.LinSpace(2f, 0f, 5);
        CollectionAssert.AreEqual(values, new float[] { 2f, 1.5f, 1f, 0.5f, 0f });

        values = MathFunctions.LinSpace(0f, 2f, 2);
        CollectionAssert.AreEqual(values, new float[] { 0f, 2f });

        values = MathFunctions.LinSpace(2f, 4f, 5);
        CollectionAssert.AreEqual(values, new float[] { 2f, 2.5f, 3f, 3.5f, 4f });

        Assert.Throws<ArgumentOutOfRangeException>(() => MathFunctions.LinSpace(0f, 2f, 1));
    }

    [TestCase(new float[] {0f, 1f, 2f, 3f}, true, true, false, false)]
    [TestCase(new float[] {0f, 0f, 2f, 3f}, true, false, false, false)]
    [TestCase(new float[] {1f, 0f, 2f, 3f}, false, false, false, false)]
    [TestCase(new float[] {0f, 3f, 2f, 4f}, false, false, false, false)]
    [TestCase(new float[] { 3f, 2f, 1f, 0f }, false, false, true, true)]
    [TestCase(new float[] { 3f, 2f, 2f, 1f }, false, false, true, false)]
    [TestCase(new float[] { 3f, 2f, 1f, 2f }, false, false, false, false)]
    public void MonotonicSequences(float[] sequence, bool isIncreasing, bool isStrictlyIncreasing, bool isDecreasing, bool isStrictlyDecreasing)
    {
        Assert.That(MathFunctions.IsIncreasing(sequence), Is.EqualTo(isIncreasing));
        Assert.That(MathFunctions.IsStrictlyIncreasing(sequence), Is.EqualTo(isStrictlyIncreasing));
        Assert.That(MathFunctions.IsDecreasing(sequence), Is.EqualTo(isDecreasing));
        Assert.That(MathFunctions.IsStrictlyDecreasing(sequence), Is.EqualTo(isStrictlyDecreasing));
    }
}