using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Ui;

public sealed class Cursor : IMoveable, IHitbox, ILayerable, IManageable
{
    private Vector2 _position;
    private readonly Vector2 _size;
    private readonly Vector2 _scale;
    private HitboxProvider _hitboxProvider;

    public Rectangle[] Hitbox => _hitboxProvider.Hitbox;

    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;

    public float Layer { get; set; }

    public bool PixelPerfect { get; set; } = false;

    public static Texture2D Texture;

    public Cursor() : this(Vector2.Zero)
    {
    }

    public Cursor(float scale) : this(Vector2.Zero, scale)
    {
    }

    public Cursor(Vector2 position, float scale = 1F)
    {
        _position = position;
        var size = new Vector2(7, 10);
        _size = size * scale;
        _scale = Vector2.One * scale;

        var box = new Rectangle(0, 0, 1, 1);
        var hitbox = new[] { box };
        _hitboxProvider = new HitboxProvider(this, hitbox, _scale);
    }

    public void Update(GameTime gameTime)
    {
        _hitboxProvider.Update(gameTime);
        _rectangle = this.GetRectangle();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            PixelPerfect ? Vector2.Floor(_position) : _position,
            null,
            Microsoft.Xna.Framework.Color.White,
            0F,
            Vector2.Zero,
            _scale,
            SpriteEffects.None,
            Layer);
    }

    public Vector2 GetPosition()
        => PixelPerfect ? Vector2.Floor(_position) : _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
    }
}