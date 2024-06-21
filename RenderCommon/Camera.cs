using OpenTK.Mathematics;
using RenderCommon.BufferObject;

namespace RenderCommon;

public class Camera
{
    private bool _equalAspectRatio = true;
    private Rect _viewDomain = Rect.FromBounds(0f, 0f, 1f, 1f);
    private Vector2 _viewportSize = new Vector2(100, 100);

    public bool EqualAspectRatio
    {
        get => _equalAspectRatio;
        set
        {
            _equalAspectRatio = value;
            UpdateProjection();
        }
    }

    public Rect ViewDomain
    {
        get => _viewDomain;
        set
        {
            _viewDomain = value;
            UpdateProjection();
        }
    }

    public Vector2 ViewportSize
    {
        get => _viewportSize;
        set
        {
            _viewportSize = value;
            UpdateProjection();
        }
    }

    public Camera()
    {
        UpdateProjection();
    }

    public Matrix4 Projection { get; private set; }

    private void UpdateProjection()
    {
        Projection = OrthographicProjection(ViewDomain, ViewportSize, EqualAspectRatio);
    }

    private static Matrix4 OrthographicProjection(Rect viewDomain, Vector2 viewportSize, bool keepAspectRatio)
    {
        float left = viewDomain.Left;
        float right = viewDomain.Right;
        float bottom = viewDomain.Bottom;
        float top = viewDomain.Top;
        if (keepAspectRatio)
        {
            var scaleX = viewDomain.Width / viewportSize.X;
            var scaleY = viewDomain.Height / viewportSize.Y;
            if (scaleX < scaleY)
            {
                right = viewportSize.X / viewportSize.Y * viewDomain.Height + left;
            }
            else
            {
                top = viewportSize.Y / viewportSize.X * viewDomain.Width + bottom;
            }

        }
        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, 1, -1);
    }
}
