using MathNet.Numerics.LinearAlgebra;
using OpenTK.Mathematics;
using RenderCommon;
using RenderCommon.BufferObject;
using System;
using System.Linq;

namespace MaterialPointMethod;

internal class VibratingBar1D
{
    private const float E = 50f;
    private const float Rho = 1f;
    private const float BarEnd = 1f;
    private const float dT = 1e-4f;
    private const float v_0 = 0.1f;
    private const float Nu = 0f;

    private static float omega = MathF.PI * MathF.Sqrt(E / Rho) / BarEnd;

    private float Time = 0f;

    public Grid1D Grid { get; }
    public Particles1D Particles { get; }
    public IConstitutiveModel1D ConstitutiveModel { get; }

    public VibratingBar1D(int ni, int np)
    {
        Grid = new Grid1D(0f, BarEnd, ni, SplineType.Linear);
        Particles = new Particles1D(Grid.Domain, np, Rho);
        
        Particles.Velocity.SetValues(Particles.Position.Select(x => ExactSolutionVelocity(x, 0)).ToArray());
        Particles.Stress.SetValues(Particles.Position.Select(x => ExactSolutionStress(x, 0)).ToArray());
        ConstitutiveModel = new NeoHookeanLargeDeformation1D(E, Nu);
    }

    public void NextStep()
    {
        var B = Grid.Basis.Sample(Particles.Position);
        var dB = Grid.Basis.Derivative(Particles.Position);

        var M = (B.PointwiseMultiply(Particles.Mass)).Transpose() * B;
        var F = -1f * (dB.Transpose() * (Particles.Stress.PointwiseMultiply(Particles.Volume)));
        var L = B.Transpose() * (Particles.Mass.PointwiseMultiply(Particles.Velocity)) + F * dT;

        Grid.ApplyHomogeneousDirichletBoundaryConditions(M, F, L);

        var A = M.Solve(F);
        var v_I = M.Solve(L);

        Particles.Velocity.SetValues((Particles.Velocity + B * A * dT).AsArray());
        Particles.Displacement.SetValues((Particles.Displacement + B * v_I * dT).AsArray());
        Particles.Position.SetValues((Particles.Position + B * v_I * dT).AsArray());

        Particles.Deformation.SetValues((1 + dB * v_I * dT).PointwiseMultiply(Particles.Deformation).AsArray());
        ConstitutiveModel.UpdateStress(Particles);
        Particles.Volume.SetValues(Particles.Deformation.PointwiseMultiply(Particles.Volume0).AsArray());

        Time += dT;
    }

    public void Draw(Camera camera, ParticleRenderer particleRender, LineRenderer lineRenderer)
    {
        var particles = MathFunctions.ParticlesCreateFromVector(Particles.Position - Particles.Displacement, Particles.Stress, 0.01f);
        float[] xVals = MathFunctions.LinSpace(0, BarEnd, 100);
        var points = xVals.Select(x => new Point2D(x, ExactSolutionStress(x, Time))).ToArray();
        var bounds = BufferObjectFunctions.GetBoundingBox(
            particles.OfType<IBoundingBox>()
            .Concat([BufferObjectFunctions.GetBoundingBox(points)]).ToArray());
        camera.ViewDomain = bounds;

        using var rectRender = new RectangleRenderer(bounds);
        rectRender.Draw(camera.Projection, Color4.White);
        lineRenderer.Draw(camera.Projection, points);
        particleRender.Draw(camera.Projection, camera.ViewportSize, Color4.Blue, particles);
    }

    private static float ExactSolutionDisplacement(float x, float t) 
        => v_0 / omega * MathF.Sin(omega * t) * MathF.Sin(MathF.PI * x / BarEnd);

    private static float ExactSolutionVelocity(float x, float t)
        => v_0 * MathF.Cos(omega * t) * MathF.Sin(MathF.PI * x / BarEnd);

    private static float ExactSolutionStress(float x, float t)
        => v_0 * E / MathF.Sqrt(E / Rho) * MathF.Sin(omega * t) * MathF.Cos(MathF.PI * x / BarEnd);
}
