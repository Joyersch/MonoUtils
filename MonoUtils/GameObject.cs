using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;

namespace MonoUtils;

public class GameObject : IHitbox, IManageable, IMoveable, IRotateable
{
    protected readonly Texture2D Texture;
    public Vector2 Position { get; protected set; }
    public Vector2 Size { get; protected set; }
    private readonly Vector2 _scale;

    protected Rectangle FramePosition;
    protected Rectangle? ImageLocation;
    public Microsoft.Xna.Framework.Color DrawColor;
    public Rectangle Rectangle { get; protected set; }
    public float Layer { get; set; }

    public float Rotation { get; set; }

    protected TextureHitboxMapping TextureHitboxMapping;
    protected Rectangle[] Hitboxes;
    protected Vector2 ScaleToTexture;
    protected OverTimeInvoker OverTimeInvoker;

    private int _currentAnimationFrame = 0;

    public Rectangle[] Hitbox => Hitboxes;

    public static Vector2 DefaultSize => new Vector2(0, 0);
    public static Texture2D DefaultTexture;

    public static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 16, 16)
        }
    };

    public GameObject() : this(Vector2.Zero)
    {
    }

    public GameObject(Vector2 position) : this(position, 1)
    {
    }

    public GameObject(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public GameObject(Vector2 position, Vector2 size) : this(position, size, DefaultTexture, DefaultMapping)
    {
    }

    public GameObject(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping)
    {
        Size = size;
        Position = position;
        DrawColor = Microsoft.Xna.Framework.Color.White;
        Texture = texture;
        TextureHitboxMapping = mapping;
        _scale = Size / mapping.ImageSize;
        ImageLocation = null;
        CalculateImageLocation();
        FramePosition = new Rectangle(Vector2.Zero.ToPoint(), TextureHitboxMapping.ImageSize.ToPoint());
        Hitboxes = new Rectangle[TextureHitboxMapping.Hitboxes.Length];
        Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
        OverTimeInvoker = new OverTimeInvoker(TextureHitboxMapping.AnimationSpeed);
        if (TextureHitboxMapping.AnimationSpeed != 0F)
            OverTimeInvoker.Trigger += CalculateImageLocation;
    }

    public virtual void Update(GameTime gameTime)
    {
        OverTimeInvoker.Update(gameTime);
        CalculateHitboxes();
        UpdateRectangle();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Position,
            ImageLocation,
            DrawColor,
            Rotation,
            TextureHitboxMapping.Origin,
            _scale,
            SpriteEffects.None,
            Layer);
    }

    protected virtual void UpdateRectangle()
        => Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());

    public bool HitboxCheck(Rectangle compareTo)
    {
        foreach (Rectangle box in Hitbox)
        {
            if (box.Intersects(compareTo))
                return true;
        }

        return false;
    }

    protected virtual void CalculateHitboxes()
    {
        ScaleToTexture = Size / TextureHitboxMapping.ImageSize;
        var textureHitboxes = TextureHitboxMapping.Hitboxes;

        for (int i = 0; i < textureHitboxes.Length; i++)
        {
            Hitboxes[i] = CalculateInGameHitbox(textureHitboxes[i]);
        }
    }

    protected void CalculateImageLocation()
    {
        if (TextureHitboxMapping.AnimationFromTop is null
            || TextureHitboxMapping.AnimationFrames <= 1)
            return;

        _currentAnimationFrame++;
        if (_currentAnimationFrame >= TextureHitboxMapping.AnimationFrames)
            _currentAnimationFrame = 0;

        var animationFromTop = TextureHitboxMapping.AnimationFromTop ?? false;
        ImageLocation = new Rectangle(
            !animationFromTop ? (int)(TextureHitboxMapping.ImageSize.X * _currentAnimationFrame + FramePosition.X) : 0,
            animationFromTop ? (int)(TextureHitboxMapping.ImageSize.Y * _currentAnimationFrame + FramePosition.Y) : 0,
            FramePosition.Width,
            FramePosition.Height);
    }

    private Rectangle CalculateInGameHitbox(Rectangle hitbox)
        => new((int)(Position.X + hitbox.X * ScaleToTexture.X)
            , (int)(Position.Y + hitbox.Y * ScaleToTexture.Y)
            , (int)(hitbox.Width * ScaleToTexture.X)
            , (int)(hitbox.Height * ScaleToTexture.Y));

    public virtual Vector2 GetPosition()
        => Position;

    public virtual Vector2 GetSize()
        => Size;

    public virtual void Move(Vector2 newPosition)
        => Position = newPosition;

    protected void MoveImageLocation(Vector2 imageLocation)
    {
        if (ImageLocation is null)
            return;

        ImageLocation = new Rectangle(imageLocation.ToPoint(), ImageLocation.Value.Size);
    }
}