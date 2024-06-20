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
        Window = new PlotWindow(800, 800, "Shape function") { Domain = new Rect(1f, 0.5f, 2f, 2f), Draw = PlotShapeFunctions };
        LineRender = new LineRenderer();
        Window.Run();
    }

    private static void PlotShapeFunctions()
    {
        var shapeFunctions = new LinearShapeFunction(MathFunctions.LinSpace(Window.Domain.Left, Window.Domain.Right, NNodes));
        var xVals = MathFunctions.LinSpace(Window.Domain.Left, Window.Domain.Right, 200);
        foreach (var index in new int[] { 0, 1, 2, 3, 4 })
        {
            LineRender.Draw(Window.Projection, xVals.Select(x => new Point2D(x, shapeFunctions.Sample(x, index))).ToArray());
        }
    }
}
