using MathNet.Numerics.LinearAlgebra;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace MaterialPointMethod;

internal class LinearBasis
{
    private float[] _nodes;
    public LinearBasis(float[] nodes)
    {
        if (!MathFunctions.IsIncreasing(nodes))
            throw new System.Exception("Nodes should be increasing.");

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
        if (index < 0) 
            throw new ArgumentOutOfRangeException();
        
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
            if (index < 0)
                throw new ArgumentOutOfRangeException();
            
            values[i, index] = (_nodes[index + 1] - x[i]) / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = (x[i] - _nodes[index]) / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }
    
    public Matrix<float> Sample(MathNet.Numerics.LinearAlgebra.Vector<float> x)
    {
        var values = _matrixBuilder.Dense(x.Count, _nodes.Length);
        for(var i = 0; i < x.Count; i++)
        {
            var index = GetLeftIndex(x[i]);
            if (index < 0)
                throw new ArgumentOutOfRangeException();
            
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
            if (index < 0)
                throw new ArgumentOutOfRangeException();

            values[i, index] = - 1f / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = 1f / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }

    public Matrix<float> Derivative(MathNet.Numerics.LinearAlgebra.Vector<float> x)
    {
        var values = _matrixBuilder.Dense(x.Count, _nodes.Length);
        for (var i = 0; i < x.Count; i++)
        {
            var index = GetLeftIndex(x[i]);
            if (index < 0)
                throw new ArgumentOutOfRangeException();

            values[i, index] = -1f / (_nodes[index + 1] - _nodes[index]);
            values[i, index + 1] = 1f / (_nodes[index + 1] - _nodes[index]);
        }
        return values;
    }

    private MatrixBuilder<float> _matrixBuilder = Matrix<float>.Build;


}

//internal class LinearBasis
//{
//    public float[] _knots;
//    public LinearBasis(float[] knots)
//    {
//        if (!MathFunctions.IsIncreasing(knots))
//            throw new System.Exception("KnotVector should be increasing.");
//
//        _knots = knots;
//    }
//}
//