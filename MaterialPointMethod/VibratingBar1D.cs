using MathNet.Numerics.LinearAlgebra;
using OpenTK.Mathematics;
using RenderCommon;
using RenderCommon.BufferObject;
using System;
using System.Linq;

namespace MaterialPointMethod;

internal class VibratingBar1D
{
    private readonly VectorBuilder<float> VecBuilder = Vector<float>.Build;
    private readonly MatrixBuilder<float> MatBuilder = Matrix<float>.Build;

    private const float E = 50f;
    private const float Rho = 1f;
    private const float BarEnd = 1f;
    private const float dT = 5e-5f;
    private const float v_0 = 0.1f;
    private const float Nu = 0f;

    private static float omega = MathF.PI * MathF.Sqrt(E / Rho) / BarEnd;
    private static (float Lambda, float Mu) Lame = (Nu * E / ((1 + Nu) * (1 - 2 * Nu)), E / (2 * (1 + Nu)));

    private readonly LinearBasis linearBasis;

    private Vector<float> Position;
    private Vector<float> Displacement;
    private Vector<float> Velocity;
    private Vector<float> Stress;
    private Vector<float> Deformation;

    private Vector<float> Volume;
    private Vector<float> Volume0;
    private Vector<float> Mass;

    private float Time = 0f;

    public VibratingBar1D(int ni, int np)
    {
        linearBasis = new LinearBasis(MathFunctions.LinSpace(0, BarEnd, ni));
        var particleDomain = MathFunctions.LinSpace(0, BarEnd, np + 1);
        Position = VecBuilder.Dense(MathFunctions.Centers(particleDomain));
        Displacement = VecBuilder.Dense(np, 0f);
        Velocity = VecBuilder.Dense(Position.Select(x => ExactSolutionVelocity(x, 0f)).ToArray());
        Stress = VecBuilder.Dense(Position.Select(x => ExactSolutionStress(x, 0f)).ToArray());
        Deformation = VecBuilder.Dense(np, 1f);

        Volume = VecBuilder.Dense(MathFunctions.Difference(particleDomain));
        Volume0 = VecBuilder.Dense(MathFunctions.Difference(particleDomain));

        Mass = VecBuilder.Dense(Volume.Select(v => Rho * v).ToArray());
    }

    public void NextStep()
    {
        var B = linearBasis.Sample(Position);
        var dB = linearBasis.Derivative(Position);

        var M = (B.PointwiseMultiply(Mass)).Transpose() * B;
        var F = -1f * (dB.Transpose() * (Stress.PointwiseMultiply(Volume)));
        var L = B.Transpose() * (Mass.PointwiseMultiply(Velocity)) + F * dT;

        ApplyDirichletBoundaryConditions(M, F, L);

        var A = M.Solve(F);
        var v_I = M.Solve(L);

        Velocity        += B * A * dT;
        Displacement    += B * v_I * dT;
        Position        += B * v_I * dT;

        var velocityGradient = dB * v_I;
        Deformation = (1 + velocityGradient * dT).PointwiseMultiply(Deformation);
        Stress = NeoHookeanModel(Deformation);
        Volume = Deformation.PointwiseMultiply(Volume0);

        Console.WriteLine($"Update time to T={Time}s");
        Time += dT;
    }

    public void Draw(Camera camera, ParticleRenderer particleRender, LineRenderer lineRenderer)
    {
        var particles = MathFunctions.ParticlesCreateFromVector(Position - Displacement, Stress, 0.01f);
        float[] xVals = MathFunctions.LinSpace(0, BarEnd, 100);
        var points = xVals.Select(x => new Point2D(x, ExactSolutionStress(x, Time))).ToArray();
        var bounds = BufferObjectFunctions.GetBoundingBox(
            particles.OfType<IBoundingBox>().Concat([BufferObjectFunctions.GetBoundingBox(points)]).ToArray());
        camera.ViewDomain = bounds;
        lineRenderer.Draw(camera.Projection, points);

        particleRender.Draw(camera.Projection, camera.ViewportSize, Color4.Blue, particles);
    }

    private void ApplyDirichletBoundaryConditions(Matrix<float> M, Vector<float> F, Vector<float> L)
    {
        // Dirichlet BC
        F[0] = 0f;
        F[F.Count - 1] = 0f;

        L[0] = 0f;
        L[L.Count - 1] = 0f;

        var ndof = F.Count;
        var Mval = M.Trace() / ndof;
        var emptyArray = new float[ndof];
        M.SetColumn(0, emptyArray);
        M.SetColumn(M.ColumnCount - 1, emptyArray);

        M.SetRow(0, emptyArray);
        M.SetRow(M.RowCount - 1, emptyArray);

        M[0, 0] = Mval;
        M[M.RowCount - 1, M.ColumnCount - 1] = Mval;
    }

    private static float ExactSolutionDisplacement(float x, float t) 
        => v_0 / omega * MathF.Sin(omega * t) * MathF.Sin(MathF.PI * x / BarEnd);

    private static float ExactSolutionVelocity(float x, float t)
        => v_0 * MathF.Cos(omega * t) * MathF.Sin(MathF.PI * x / BarEnd);

    private static float ExactSolutionStress(float x, float t)
        => v_0 * E / MathF.Sqrt(E / Rho) * MathF.Sin(omega * t) * MathF.Cos(MathF.PI * x / BarEnd);

    private static Vector<float> NeoHookeanModel(Vector<float> F) 
        => Vector<float>.Build.Dense(F.Select(NeoHookeanModel).ToArray());


    private static float NeoHookeanModel(float F) => Lame.Lambda / F * MathF.Log(F) + Lame.Mu * (F * F - 1) / F;
}
