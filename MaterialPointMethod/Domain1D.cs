using System;

namespace MaterialPointMethod;

internal class Domain1D
{
    public float Left { get; }
    public float Right { get; }

    public Domain1D(float left, float right)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(left, right);

        Left = left;
        Right = right;
    }
}
