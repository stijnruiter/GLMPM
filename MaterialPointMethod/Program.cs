using RenderCommon;
using RenderCommon.BufferObject;
using System.Linq;

namespace MaterialPointMethod;

internal class Program
{
    private const int NNodes = 5;
    private static LineRenderer LineRender;
    private static PlotWindow Window;

    static void Main(string[] args)
    {
        using(var render = new Window(800, 800, "Material Point Method"))
        {
            render.Run();
        }

        Window = new PlotWindow(800, 800, "Shape function") { Draw = PlotShapeFunctions };
        Window.Camera.ViewDomain = new Rect(1f, 0.5f, 2f, 2f);
        LineRender = new LineRenderer();
        Window.Run();
    }

    private static void PlotShapeFunctions()
    {
        var shapeFunctions = new LinearShapeFunction(MathFunctions.LinSpace(Window.Camera.ViewDomain.Left, Window.Camera.ViewDomain.Right, NNodes));
        var xVals = MathFunctions.LinSpace(Window.Camera.ViewDomain.Left, Window.Camera.ViewDomain.Right, 200);
        foreach (var index in new int[] { 0, 1, 2, 3, 4 })
        {
            LineRender.Draw(Window.Camera.Projection, xVals.Select(x => new Point2D(x, shapeFunctions.Sample(x, index))).ToArray());
        }
    }
}
