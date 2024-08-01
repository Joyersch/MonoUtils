using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Ui;

public sealed class Camera : IMoveable, IHitbox
{
    public Matrix CameraMatrix { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 RealPosition { get; private set; }
    public Vector2 Size { get; private set; }
    public Vector2 RealSize { get; private set; }
    public Rectangle Rectangle { get; private set; }

    public float Zoom = 2f;

    public Rectangle[] Hitbox => new[] { Rectangle };

    public Camera(Vector2 position, Vector2 size)
    {
        Size = size;
        Position = position;
        RealPosition = Position - Size / Zoom / 2;
        RealSize = Size / Zoom;
        Rectangle = new Rectangle(RealPosition.ToPoint(), RealSize.ToPoint());
    }

    public void Update()
    {
        UpdateMatrix();
        RealPosition = Position - Size / Zoom / 2;
        RealSize = Size / Zoom;
        Rectangle = new Rectangle(RealPosition.ToPoint(), RealSize.ToPoint());
    }

    private void UpdateMatrix()
    {
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
        Update();
    }
}