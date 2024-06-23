using MathNet.Numerics.LinearAlgebra;
using System;
using System.Linq;

namespace MaterialPointMethod;

internal interface IConstitutiveModel1D
{
    public Vector<float> ComputeStress(Particles1D particles);
    public void UpdateStress(Particles1D particles);
}

internal class NeoHookeanLargeDeformation1D : IConstitutiveModel1D
{
    public float E { get; set; }
    public float Nu { get; set; }

    public NeoHookeanLargeDeformation1D(float E, float Nu)
    {
        this.E = E;
        this.Nu = Nu;
    }

    private (float Lambda, float Mu) Lame => (Nu * E / ((1 + Nu) * (1 - 2 * Nu)), E / (2 * (1 + Nu)));

    public Vector<float> ComputeStress(Particles1D particles)
        => Vector<float>.Build.Dense(particles.Deformation.Select(NeoHookeanModel).ToArray());

    public void UpdateStress(Particles1D particles)
    {
        for (var i = 0; i < particles.Count; i++)
        {
            particles.Stress[i] = NeoHookeanModel(particles.Deformation[i]);
        }
    }

    private float NeoHookeanModel(float F) => Lame.Lambda / F * MathF.Log(F) + Lame.Mu * (F * F - 1) / F;
}

