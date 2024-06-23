using MathNet.Numerics.LinearAlgebra;
using System;

namespace MaterialPointMethod;

internal class Grid1D
{
    public Domain1D Domain { get; }
    public Vector<float> Nodes { get; }
    public SplineType BasisType { get; }

    public BasisFunction Basis { get; }
    
    public Grid1D(Domain1D domain, int nodeCount, SplineType basisFunction)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(nodeCount, 2);

        Domain = domain;
        Nodes = Vector<float>.Build.Dense(MathFunctions.LinSpace(domain.Left, domain.Right, nodeCount));
        Basis = BuildBasisFunctions();
    }

    public Grid1D(float left, float right, int nodeCount, SplineType basisFunction) 
        : this(new Domain1D(left, right), nodeCount, basisFunction) { }

    private BasisFunction BuildBasisFunctions()
    {
        switch(BasisType)
        {
            case SplineType.Linear:
                return new LinearBasis(Nodes.AsArray());
            default:
                throw new NotImplementedException();
        }
    }

    public int Ndof => Nodes.Count;

    private int[] BoundaryIndices => new int[] { 0, Nodes.Count - 1 };

    public void ApplyHomogeneousDirichletBoundaryConditions(Matrix<float> M, Vector<float> F, Vector<float> L)
    {
        var Mval = M.Trace() / Ndof;
        foreach(var bcIndex in BoundaryIndices)
        {
            F[bcIndex] = 0f;
            L[bcIndex] = 0f;
            M.ClearColumn(bcIndex);
            M.ClearRow(bcIndex);
            M[bcIndex, bcIndex] = Mval;
        }
    }
}
