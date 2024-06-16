using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
        ParticleRenderer = new ParticleRenderer();
        Particles = [
            new Particle2D(10f, -3f, .5f),
            new Particle2D(15f, 0f, .25f),
            new Particle2D(20f, 3f, 1.0f),
        ];
        Domain = GetContainingRectangle(Particles, 1f);
        UpdateProjection();
        RectangleRenderer = new RectangleRenderer(Domain);
        GridRenderer = new GridRenderer(Domain);
    }


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        RectangleRenderer.Draw(Projection);
        GridRenderer.Draw(Projection);
        ParticleRenderer.Draw(Projection, ViewportSize, Particles);

        SwapBuffers();
    }


    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        ViewportSize = (e.Width, e.Height);
        GL.Viewport(0, 0, e.Width, e.Height);
        UpdateProjection();
    }

    private void UpdateProjection()
    {
        Projection = OrthographicProjection(Domain);
    }

    private Matrix4 OrthographicProjection(Rect frame, bool keepAspectRatio = true)
    {
        float left      = frame.CX - frame.Width / 2;
        float right     = frame.CX + frame.Width / 2;
        float bottom    = frame.CY - frame.Height / 2;
        float top       = frame.CY + frame.Height / 2;
        if (keepAspectRatio)
        {
            var scaleX = frame.Width / ViewportSize.X;
            var scaleY = frame.Height / ViewportSize.Y;
            if (scaleX < scaleY)
            {
                right = ViewportSize.X / ViewportSize.Y * frame.Height + left;
            }
            else
            {
                top = ViewportSize.Y / ViewportSize.X * frame.Width + bottom;
            }

        }
        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
    }

    private Rect GetContainingRectangle(Particle2D[] particles, float expand = 0.0f)
    {
        var l = float.MaxValue;
        var r = float.MinValue;
        var b = float.MaxValue;
        var t = float.MinValue;
        foreach (var particle in particles)
        {
            l = Math.Min(l, particle.X - particle.Size / 2);
            r = Math.Max(r, particle.X + particle.Size / 2);
            b = Math.Min(b, particle.Y - particle.Size / 2);
            t = Math.Max(t, particle.Y + particle.Size / 2);
        }
        var rect = Rect.FromBounds(l, t, r, b);
        rect.Expand(expand);
        return rect;
    }

    private Matrix4 Projection { get; set; }

    private Rect Domain { get; set; }

    private Vector2 ViewportSize { get; set; }

    private Particle2D[] Particles { get; set; }

    private ParticleRenderer ParticleRenderer { get; set; }

    private RectangleRenderer RectangleRenderer { get; set; }

    private GridRenderer GridRenderer { get; set; }

}
