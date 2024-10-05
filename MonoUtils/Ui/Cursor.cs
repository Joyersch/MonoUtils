using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Ui;

public sealed class Cursor : IMoveable, IHitbox, ILayerable, IManageable, IScaleable
{
    private Vector2 _position;
    private readonly float _initialScale;
    private float _extendedScale = 1F;
    private Vector2 _drawingScale;
    public float Scale => _initialScale * _extendedScale;
    private Vector2 _baseSize = new Vector2(7, 10);
    private Vector2 _size;

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

    public Cursor(float initialScale) : this(Vector2.Zero, initialScale)
    {
    }

    public Cursor(Vector2 position, float initialScale = 1F)
    {
        _position = position;
        _initialScale = initialScale;
        _size = _baseSize * Scale;
        _drawingScale = Vector2.One * Scale;

        var box = new Rectangle(0, 0, 1, 1);
        var hitbox = new[] { box };
        _hitboxProvider = new HitboxProvider(this, hitbox, _drawingScale);
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
            _drawingScale,
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

    public void SetScale(float scale)
    {
        _extendedScale = scale;
        _size = _baseSize * Scale;
        _drawingScale = Vector2.One * Scale;
        _rectangle = this.GetRectangle();
        _hitboxProvider.SetScale(_drawingScale);
    }
}