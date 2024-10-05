using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Ui.Buttons;

public class RatioBox : IButton
{
    private readonly RatioGroup _group;
    private readonly int _groupId;
    private Vector2 _position;
    private readonly float _initialScale;
    private float _extendedScale = 1F;
    private Vector2 _size;
    private Vector2 _drawingScale;

    public float Scale => _extendedScale * _extendedScale;
    private Microsoft.Xna.Framework.Color _color;
    private Rectangle _imageLocation;

    public bool Selected { get; private set; }

    private MouseActionsMat _mouseMat;
    private HitboxProvider _hitbox;

    public Rectangle[] Hitbox => _hitbox.Hitbox;
    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;
    public float Layer { get; set; }

    public bool IsHover => _mouseMat.IsHover;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public static Texture2D Texture;
    private static Vector2 ImageSize { get; set; } = new Vector2(8, 8);

    public static float DefaultScale { get; set; } = 3F;

    public RatioBox(RatioGroup group) : this(group, DefaultScale)
    {
    }

    public RatioBox(RatioGroup group, float initialScale) : this(group, Vector2.Zero, initialScale)
    {
    }

    public RatioBox(RatioGroup group, Vector2 position, float initialScale)
    {
        _group = group;
        _group.UpdateStatus += delegate(int i)
        {
            Selected = _groupId == i;
            _imageLocation = new Rectangle((int)(Selected ? ImageSize.X : 0), 0, (int)ImageSize.X, (int)ImageSize.Y);
        };
        _groupId = _group.Register(this);

        _position = position;
        _initialScale = initialScale;
        _size = ImageSize * Scale;
        _drawingScale = Vector2.One * Scale;
        _color = Microsoft.Xna.Framework.Color.White;

        var hitbox = new[]
        {
            new Rectangle(1, 0, 6, 8),
            new Rectangle(0, 1, 8, 6)
        };
        _hitbox = new HitboxProvider(this, hitbox, _drawingScale);
        _rectangle = this.GetRectangle();

        _mouseMat = new MouseActionsMat(this);
        _mouseMat.Leave += _ => Leave?.Invoke(this);
        _mouseMat.Enter += _ => Enter?.Invoke(this);
        _mouseMat.Click += delegate
        {
            if (!Selected)
                Click?.Invoke(this);
            _group.Select(this);
        };
        _imageLocation = new Rectangle((int)(Selected ? ImageSize.X : 0), 0, (int)ImageSize.X, (int)ImageSize.Y);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseMat.UpdateInteraction(gameTime, toCheck);
    }

    public void Update(GameTime gameTime)
    {
        _hitbox.Update(gameTime);
        _rectangle = this.GetRectangle();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            _imageLocation,
            _color,
            0F,
            Vector2.Zero,
            _drawingScale,
            SpriteEffects.None,
            Layer);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
        _rectangle = this.GetRectangle();
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        _color = input[0];
    }

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => [_color];
    
    public void SetScale(float scale)
    {
        _extendedScale = scale;
        _size = ImageSize * Scale;
        _drawingScale = Vector2.One * Scale;
        _rectangle = this.GetRectangle();
    }
}