using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RenderCommon.BufferObject;

namespace RenderCommon;

public class Window : GameWindow
{
    public Window(int width, int height, string title)
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Size = (width, height);
        Title = title;
        UpdateFrequency = 60;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

        CreateBufferObjects();

        var viewDomain = BufferObjectFunctions.GetBoundingBox(Particles.OfType<IBoundingBox>().ToArray());
        viewDomain.Dilate(1f);
        Camera.ViewDomain = viewDomain;

        ParticleRenderer = new ParticleRenderer();
        RectangleRenderer = new RectangleRenderer(viewDomain);
        GridRenderer = new GridRenderer(viewDomain);
        LineRenderer = new LineRenderer();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        RectangleRenderer.Draw(Camera.Projection, Color4.White);
        GridRenderer.Draw(Camera.Projection, Color4.Black);
        ParticleRenderer.Draw(Camera.Projection, Camera.ViewportSize, Color4.Blue, Particles);
        LineRenderer.Draw(Camera.Projection, Line);
        LineRenderer.Draw(Camera.Projection, Lines);

        SwapBuffers();
    }

private double Time;

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Time += args.Time;

        Particles[0].Position.X = 10f + (float)Math.Sin(Time);
        Particles[1].Position.X = 15f + (float)Math.Sin(Time);
        Particles[2].Position.X = 20f + (float)Math.Sin(Time);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        Camera.ViewportSize = (e.Width, e.Height);
    }

    private void CreateBufferObjects()
    {
        Particles =
        [
            new Particle2D(10f, -3f, .5f),
            new Particle2D(15f, 0f, .25f),
            new Particle2D(20f, 3f, 1.0f),
        ];
        Line =
        [
            new Point2D(0f, 0f),
            new Point2D(10f, -3f),
            new Point2D(15f, 0f),
            new Point2D(20f, 3f)
        ];
        Lines = 
        [
            new Line2D(0f, 0f, 1f, 1f),
            new Line2D(10f, 0f, 20f, 1f),
            new Line2D(15f, 3f, 0f, 3f),
            new Line2D(20f, -3f, 10f, 3f),
        ];
    }

    private Camera Camera { get; } = new Camera();

    private Particle2D[] Particles { get; set; }
    private Point2D[] Line { get; set; }
    private Line2D[] Lines { get; set; }

    private ParticleRenderer ParticleRenderer { get; set; }

    private RectangleRenderer RectangleRenderer { get; set; }

    private GridRenderer GridRenderer { get; set; }

    private LineRenderer LineRenderer { get; set; }

}
