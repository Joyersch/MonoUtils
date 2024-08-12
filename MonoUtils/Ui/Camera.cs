using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Ui;

public sealed class Camera : IMoveable, IHitbox, IUpdateable
{
    public Matrix CameraMatrix { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 RealPosition { get; private set; }
    public Vector2 Size { get; private set; }
    public Vector2 RealSize { get; private set; }

    /// <summary>
    /// The visible area of the camera
    /// </summary>
    public Rectangle Rectangle { get; private set; }

    public float Zoom => _zoom;

    private float _zoom = 1F;
    private float _zoomDifferance;
    private float _zoomIntent;

    public float ZoomSpeed = 100F;

    public Rectangle[] Hitbox => new[] { Rectangle };


    public Camera(Display display) : this(display.Size)
    {
    }

    public Camera(Vector2 size) : this(Vector2.Zero, size)
    {
    }

    public Camera(Vector2 position, Vector2 size)
    {
        Size = size;
        Position = position;
        Calculate();
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        var factor = _zoomDifferance * (deltaTime / ZoomSpeed);

        var nextZoom = _zoom + factor;
        if ((_zoomDifferance > 0 && nextZoom > _zoomIntent) ||
            (_zoomDifferance < 0 && nextZoom < _zoomIntent))
            _zoom = _zoomIntent;
        else
            _zoom = nextZoom;
    }

    public void Calculate()
    {
        RealPosition = Position - Size / Zoom / 2;
        RealSize = Size / Zoom;
        Rectangle = new Rectangle(RealPosition.ToPoint(), RealSize.ToPoint());

        // Update matrix
        var position = Matrix.CreateTranslation(-Position.X, -Position.Y, 0);
        var offset = Matrix.CreateTranslation(Size.X / 2, Size.Y / 2, 0);
        CameraMatrix = position * Matrix.CreateScale(Zoom) * offset;
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        Position = newPosition;
        Calculate();
    }

    public void SetZoom(float zoom)
    {
        _zoomIntent = zoom;
        _zoomDifferance = zoom - Zoom;
    }
}