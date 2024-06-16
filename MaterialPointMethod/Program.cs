namespace MaterialPointMethod;

internal class Program
{ 
    static void Main(string[] args)
    {
        var renderer = new RenderCommon.Window(800, 800, "Material Point Method");
        renderer.Run();
    }
}
