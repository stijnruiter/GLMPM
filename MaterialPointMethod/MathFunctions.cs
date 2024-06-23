using MathNet.Numerics.LinearAlgebra;
using RenderCommon.BufferObject;
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

    public static float[] Difference(float[] vector)
    {
        var differences = new float[vector.Length - 1];
        for (var i = 0; i < differences.Length; i++)
        {
            differences[i] = vector[i + 1] - vector[i];
        }
        return differences;
    }

    public static float[] Centers(float[] vector)
    {
        var centers = new float[vector.Length - 1];
        for (var i = 0; i < centers.Length; i++)
        {
            centers[i] = (vector[i + 1] + vector[i]) / 2;
        }
        return centers;
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

    public static Matrix<float> PointwiseMultiply(this Matrix<float> mat, Vector<float> rhs)
    {
        var returnMat = Matrix<float>.Build.Dense(mat.RowCount, mat.ColumnCount);
        for (var i = 0; i < mat.ColumnCount; i++)
        {
            returnMat.SetColumn(i, mat.Column(i).PointwiseMultiply(rhs));
        }
        return returnMat;
    }

    public static Particle2D[] ParticlesCreateFromVector(Vector<float> x, Vector<float> y, float size)
    {
        if (x.Count != y.Count)
            throw new ArgumentException($"Array x and y not of same length, {x.Count} != {y.Count}");
        var values = new Particle2D[x.Count];
        for (var i = 0; i < x.Count; i++)
        {
            values[i].Position.X = x[i];
            values[i].Position.Y = y[i];
            values[i].Size = size;
        }
        return values;
    }
}
