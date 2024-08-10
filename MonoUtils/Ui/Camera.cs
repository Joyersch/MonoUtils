using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Ui;

public sealed class Camera : IMoveable, IHitbox
{
    public Matrix CameraMatrix { get; private set; }

    public Vector2 FullPosition { get; private set; }
    public Vector2 FullSize { get; private set; }
    public Rectangle FullBounds { get; private set; }

    public Vector2 VisiblePosition { get; private set; }
    public Vector2 VisibleSize { get; private set; }
    public Rectangle VisibleBounds { get; private set; }

    public float Zoom = 2f;

    public Rectangle[] Hitbox => new[] { VisibleBounds };

    public Camera(Vector2 fullPosition, Vector2 fullSize)
    {
        FullSize = fullSize;
        FullPosition = fullPosition;
        FullBounds = new Rectangle(FullPosition.ToPoint(), FullSize.ToPoint());

        VisiblePosition = FullPosition - FullSize / Zoom / 2;
        VisibleSize = FullSize / Zoom;
        VisibleBounds = new Rectangle(VisiblePosition.ToPoint(), VisibleSize.ToPoint());
    }

    public void Update()
    {
        UpdateMatrix();
        FullBounds = new Rectangle(FullPosition.ToPoint(), FullSize.ToPoint());
        VisiblePosition = FullPosition - FullSize / Zoom / 2;
        VisibleSize = FullSize / Zoom;
        VisibleBounds = new Rectangle(VisiblePosition.ToPoint(), VisibleSize.ToPoint());
    }

    private void UpdateMatrix()
    {
        var position = Matrix.CreateTranslation(-FullPosition.X, -FullPosition.Y, 0);
        var offset = Matrix.CreateTranslation(FullSize.X / 2, FullSize.Y / 2, 0);
        CameraMatrix = position * Matrix.CreateScale(Zoom) * offset;
    }

    public Vector2 GetPosition()
        => FullPosition;

    public Vector2 GetSize()
        => FullSize;

    public void Move(Vector2 newPosition)
    {
        FullPosition = newPosition;
        Update();
    }

    public void SetSize(Vector2 size)
    {
        FullSize = size;
        Update();
    }
}