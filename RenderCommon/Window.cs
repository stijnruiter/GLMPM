using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RenderCommon;

public class Window : GameWindow
{
    private Matrix4 ViewMatrix { get; set; }

    private Particle2D[] _particles;
    private RectangleRenderer _rectangleRenderer;
    private ParticleRenderer _particleRenderer;
    private GridRenderer _gridRenderer;
    private Rect _domain;
    private Vector2 ViewportSize;

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
        _particleRenderer = new ParticleRenderer();
        _particles = [
            new Particle2D(10f, -3f, .5f),
            new Particle2D(15f, 0f, .25f),
            new Particle2D(20f, 3f, 1.0f),
        ];
        _domain = GetContainingRectangle(_particles, 1f);
        UpdateViewMatrix();
        _rectangleRenderer = new RectangleRenderer(_domain);
        _gridRenderer = new GridRenderer(_domain);
    }

    private Rect GetContainingRectangle(Particle2D[] particles, float expand = 0.0f)
    {
        var l =  float.MaxValue;
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

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _rectangleRenderer.Draw(ViewMatrix);
        _gridRenderer.Draw(ViewMatrix);
        _particleRenderer.Draw(ViewMatrix, ViewportSize, _particles);

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
        UpdateViewMatrix();
    }

    private void UpdateViewMatrix()
    {
        ViewMatrix = OrthoView(_domain);
    }

    private Matrix4 OrthoView(Rect frame, bool keepAspectRatio = true)
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
}
