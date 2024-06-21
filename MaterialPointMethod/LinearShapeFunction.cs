using System;

namespace MaterialPointMethod;

internal class LinearShapeFunction
{
    public LinearShapeFunction(float[] nodes)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(nodes.Length, 2);
        
        if (!MathFunctions.IsIncreasing(nodes))
            throw new ArgumentException("Nodes array should be increasing.");

        _nodes = nodes;
    }

    public float Sample(float x, int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(x, _nodes[0]);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(x, _nodes[_nodes.Length - 1]);

        if (x < _nodes[index])
            return Math.Max(0f, (x - _nodes[index - 1]) / (_nodes[index] - _nodes[index - 1]));

        if (x > _nodes[index])
            return Math.Max(0f, (x - _nodes[index + 1]) / (_nodes[index] - _nodes[index + 1]));
        
        return 1;
    }

    private readonly float[] _nodes;
}
