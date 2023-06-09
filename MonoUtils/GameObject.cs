﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;

namespace MonoUtils;

public class GameObject : IHitbox, IManageable, IMoveable
{
    protected readonly Texture2D Texture;
    public Vector2 Position { get; protected set; }
    public Vector2 Size { get; protected set; }
    protected Vector2 FrameSize;
    protected Rectangle ImageLocation;
    public Microsoft.Xna.Framework.Color DrawColor;
    public Rectangle Rectangle { get; protected set; }

    protected TextureHitboxMapping TextureHitboxMapping;
    protected Rectangle[] Hitboxes;
    protected Vector2 ScaleToTexture;

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

    public GameObject(Vector2 position, float scale) : this(position,DefaultSize * scale)
    {
    }
    
    public GameObject(Vector2 position, Vector2 size) : this(position, size,  DefaultTexture, DefaultMapping)
    {
    }

    public GameObject(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping)
    {
        Size = size;
        Position = position;
        DrawColor = Microsoft.Xna.Framework.Color.White;
        Texture = texture;
        TextureHitboxMapping = mapping;
        ImageLocation = new Rectangle(0, 0
            , (int) TextureHitboxMapping.ImageSize.X, (int) TextureHitboxMapping.ImageSize.Y);
        FrameSize = TextureHitboxMapping.ImageSize;
        Hitboxes = new Rectangle[TextureHitboxMapping.Hitboxes.Length];
        Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
    }

    public virtual void Update(GameTime gameTime)
    {
        CalculateHitboxes();
        UpdateRectangle();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (ImageLocation == Rectangle.Empty)
            spriteBatch.Draw(Texture, Rectangle, DrawColor);
        else
            spriteBatch.Draw(Texture, Rectangle, ImageLocation, DrawColor);
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

    private Rectangle CalculateInGameHitbox(Rectangle hitbox)
        => new((int) (Position.X + hitbox.X * ScaleToTexture.X)
            , (int) (Position.Y + hitbox.Y * ScaleToTexture.Y)
            , (int) (hitbox.Width * ScaleToTexture.X)
            , (int) (hitbox.Height * ScaleToTexture.Y));

    public virtual Vector2 GetPosition()
        => Position;

    public virtual Vector2 GetSize()
        => Size;

    public virtual void Move(Vector2 newPosition)
        => Position = newPosition;
}