using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RenderCommon.BufferObject;

namespace RenderCommon;

public class PlotWindow : GameWindow
{
    public PlotWindow(int width, int height, string title)
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Size = (width, height);
        Title = title;
        UpdateFrequency = 60;
        GL.ClearColor(Background);
    }

    private Color4 Background { get; set; } = Color4.White;
    public Action Draw { get; set; }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        Draw?.Invoke();
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
        float left = frame.Center.X - frame.Width / 2;
        float right = frame.Center.X + frame.Width / 2;
        float bottom = frame.Center.Y - frame.Height / 2;
        float top = frame.Center.Y + frame.Height / 2;
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

    public Matrix4 Projection { get; private set; }

    private Rect _domain;
    public Rect Domain 
    { 
        get => _domain;
        set
        {
            _domain = value;
            UpdateProjection();
        }
    }

    public Vector2i ViewportSize { get; private set; }

}
