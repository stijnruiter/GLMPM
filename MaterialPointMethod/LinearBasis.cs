using MathNet.Numerics.LinearAlgebra;
using System;
using System.Diagnostics;

namespace MaterialPointMethod;

public enum SplineType
{
    Linear
}

public interface BasisFunction
{
    public SplineType Type { get; }
    public Matrix<float> Sample(Vector<float> x);
    public Matrix<float> Derivative(Vector<float> x);
}

internal class LinearBasis : BasisFunction
{
    public LinearBasis(float[] nodes)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(nodes.Length, 2);

        if (!MathFunctions.IsIncreasing(nodes))
            throw new ArgumentException("Nodes should be increasing.");

        _nodes = nodes;
    }

    private int GetLeftIndex(float x)
    {
        if (x < _nodes[0] || x > _nodes[_nodes.Length - 1])
            return -1;

        for (var index = _nodes.Length - 1; index >= 0; index--)
        {
            // if true, then _nodes[index] < x <= _nodes[index + 1]
            if (x > _nodes[index])
                return index;
        }

        // First interval _nodes[0] <= x <= _nodes[1];
        Debug.Assert(_nodes[0] == x);
        return 0;
    }

    public float[] Sample(float x)
    {
        var index = GetLeftIndex(x);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        var values = new float[_nodes.Length];
        values[index] = (_nodes[index + 1] - x) / (_nodes[index + 1] - _nodes[index]);
        values[index + 1] = (x - _nodes[index]) / (_nodes[index + 1] - _nodes[index]);
        return values;
    }

    public Matrix<float> Sample(float[] x)
    {
        var values = _matrixBuilder.Dense(x.Length, _nodes.Length);
        for(var i = 0; i < x.Length; i++)
        {
            var index = GetLeftIndex(x[i]);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            values[i, index] = (_nodes[index + 1] - x[i]) / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = (x[i] - _nodes[index]) / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }
    
    public Matrix<float> Sample(Vector<float> x)
    {
        var values = _matrixBuilder.Dense(x.Count, _nodes.Length);
        for(var i = 0; i < x.Count; i++)
        {
            var index = GetLeftIndex(x[i]);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            values[i, index] = (_nodes[index + 1] - x[i]) / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = (x[i] - _nodes[index]) / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }

    public Matrix<float> Derivative(float[] x)
    {
        var values = _matrixBuilder.Dense(x.Length, _nodes.Length);
        for (var i = 0; i < x.Length; i++)
        {
            var index = GetLeftIndex(x[i]);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            values[i, index] = - 1f / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = 1f / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }

    public Matrix<float> Derivative(Vector<float> x)
    {
        var values = _matrixBuilder.Dense(x.Count, _nodes.Length);
        for (var i = 0; i < x.Count; i++)
        {
            var index = GetLeftIndex(x[i]);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            values[i, index] = -1f / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = 1f / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }

    private float[] _nodes;
    private MatrixBuilder<float> _matrixBuilder = Matrix<float>.Build;

    public SplineType Type { get; } = SplineType.Linear;
}