using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUtils.Ui.Objects.Buttons;

public class MiniTextButton : TextButton
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize * 4;
    public new static float DefaultTextScale => 2F;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 8),
        Hitboxes = new[]
        {
            new Rectangle(0, 1, 16, 6),
            new Rectangle(1, 0, 14, 8)
        }
    };

    public MiniTextButton(string text) : this(Vector2.Zero, text)
    {
    }
    
    public MiniTextButton(string text, float scale) : this(Vector2.Zero, scale, string.Empty, text)
    {
    }

    public MiniTextButton(Vector2 position, string text) : this(position, string.Empty, text)
    {
    }

    public MiniTextButton(Vector2 position, string name, string text) : this(position, 1, name, text)
    {
    }

    public MiniTextButton(Vector2 position, float scale, string name, string text) : this(position,
        DefaultSize * scale, name, text, DefaultTextScale)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, float textScale) : this(position,
        size, name, text, textScale, 1)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, float textScale, int spacing) :
        this(position, size, name, text, textScale, spacing, DefaultTexture, DefaultMapping)
    {
    }

    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, float textScale, int spacing,
        Texture2D texture, TextureHitboxMapping mapping) : base(position, size, name, text, textScale, spacing, texture,
        mapping)
    {
    }
}