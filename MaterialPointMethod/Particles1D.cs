using MathNet.Numerics.LinearAlgebra;
using System.Linq;

namespace MaterialPointMethod;

internal class Particles1D
{
    public int Count { get; }

    public Vector<float> Position { get; }
    public Vector<float> Displacement { get; }
    public Vector<float> Velocity { get; }
    public Vector<float> Stress { get; }
    public Vector<float> Deformation { get; }
    public Vector<float> Mass { get; }
    public Vector<float> Volume { get; }
    public Vector<float> Volume0 { get; }

    public Particles1D(Domain1D domain, int particleCount, float density = 0f)
    {
        Count = particleCount;

        var particleEdges = MathFunctions.LinSpace(domain.Left, domain.Right, particleCount + 1);
        Position = Vector<float>.Build.Dense(MathFunctions.Centers(particleEdges));
        Volume = Vector<float>.Build.Dense(MathFunctions.Difference(particleEdges));
        Volume0 = Vector<float>.Build.Dense(MathFunctions.Difference(particleEdges));
        Deformation = Vector<float>.Build.Dense(particleCount, 1f);
        Displacement = Vector<float>.Build.Dense(particleCount, 0f);
        Velocity = Vector<float>.Build.Dense(particleCount, 0f);
        Stress = Vector<float>.Build.Dense(particleCount, 0f);
        
        Mass = Vector<float>.Build.Dense(Volume.Select(v => v * density).ToArray());
    }

}
