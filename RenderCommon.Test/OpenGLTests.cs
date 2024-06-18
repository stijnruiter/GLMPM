using NUnit.Framework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using Imaging = System.Drawing.Imaging;

namespace RenderCommon.Test;

[TestFixture]
internal abstract class OpenGLTests
{
    protected NativeWindowSettings NativeWindowSettings { get; private set; }
    protected NativeWindow Window { get; private set; }

    protected virtual int Width => 400;
    protected virtual int Height => 400;
    protected virtual Color BackgroundColor => Color.Black;

    [SetUp]
    public void CreateWindow()
    {
        // Not clean, but does make it possible to use GL for simple unit testing of the rendering code output
        GLFWProvider.CheckForMainThread = false;

        NativeWindowSettings = NativeWindowSettings.Default;
        NativeWindowSettings.StartVisible = false;
        NativeWindowSettings.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Hidden;

        Window = new NativeWindow(NativeWindowSettings);
        Window.Size = (Width, Height);
        GL.Viewport(0, 0, Window.FramebufferSize.X, Window.FramebufferSize.Y);

        GL.ClearColor(BackgroundColor);
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    [TearDown]
    public void DeleteWindow() => Window?.Dispose();


    protected Bitmap CreateBitmap()
    {
        Bitmap bitmap = new Bitmap(Window.FramebufferSize.X, Window.FramebufferSize.Y);
        var bits = bitmap.LockBits(new Rectangle(0, 0, Window.FramebufferSize.X, Window.FramebufferSize.Y), Imaging.ImageLockMode.WriteOnly, Imaging.PixelFormat.Format32bppArgb);
        GL.ReadPixels(0, 0, Window.FramebufferSize.X, Window.FramebufferSize.Y, PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
        bitmap.UnlockBits(bits);
        return bitmap;
    }
}
