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
        GL.Viewport(0, 0, e.Width, e.Height);
        Camera.ViewportSize = (e.Width, e.Height);
    }

    public Camera Camera { get; } = new Camera();

}
