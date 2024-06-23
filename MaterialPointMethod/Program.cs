using RenderCommon;

namespace MaterialPointMethod;

internal class Program
{
    private static LineRenderer LineRender;
    private static PlotWindow Window;
    private static ParticleRenderer ParticleRenderer;

    static void Main(string[] args)
    {
        var vibBarSimulation = new VibratingBar1D(11, 100);
        Window = new PlotWindow(800, 800, "Vibrating bar")
        {
            Background = OpenTK.Mathematics.Color4.Black,
            Draw = () =>
            {
                vibBarSimulation.NextStep();
                vibBarSimulation.Draw(Window.Camera, ParticleRenderer, LineRender);
            }
        };
        LineRender = new LineRenderer();
        ParticleRenderer = new ParticleRenderer();
        Window.Run();
    }
}
