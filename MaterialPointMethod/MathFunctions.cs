using System;

namespace MaterialPointMethod;

public static class MathFunctions
{
    public static float[] LinSpace(float start, float end, int n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 2);
        var values = new float[n];
        var step = (end - start) / (n - 1);
        for (int i = 0; i < n; i++)
        {
            values[i] = start + step * i;
        }
        return values;
    }

    public static bool IsIncreasing(float[] sequence) => CompareSequence(sequence, (x, y) => x <= y);

    public static bool IsStrictlyIncreasing(float[] sequence) => CompareSequence(sequence, (x, y) => x < y);
    
    public static bool IsDecreasing(float[] sequence) => CompareSequence(sequence, (x, y) => x >= y);
    
    public static bool IsStrictlyDecreasing(float[] sequence) => CompareSequence(sequence, (x, y) => x > y);

    private static bool CompareSequence(float[] sequence, Func<float, float, bool> sequenceCondition)
    {
        if (sequence.Length < 2)
            return true;

        for (int i = 0; i < sequence.Length - 1; i++)
        {
            if (!sequenceCondition(sequence[i], sequence[i+1]))
            {
                return false;
            }
        }
        return true;
    }
}
