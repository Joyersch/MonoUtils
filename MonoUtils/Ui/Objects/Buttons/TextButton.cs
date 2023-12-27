using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Ui.Color;

namespace MonoUtils.Ui.Objects.Buttons;

public class TextButton : EmptyButton, IColorable
{
    public TextSystem.Text Text { get; }
    public string Name { get; }
    public new static Vector2 DefaultSize => new Vector2(128, 64);
    public static float DefaultTextScale => 2F;

    public TextButton(string text) : this(Vector2.Zero, string.Empty, text)
    {
    }


    public TextButton(string text, float scale) : this(text, string.Empty, scale)
    {
    }

    public TextButton(string text, float scale, float textScale) :
        this(Vector2.Zero, DefaultSize * scale, string.Empty, text, textScale)
    {
    }

    public TextButton(string text, string name) : this(Vector2.Zero, name, text)
    {
    }

    public TextButton(string text, string name, float scale) : this(Vector2.Zero, scale, name, text)
    {
    }

    public TextButton(Vector2 position, string text) : this(position, string.Empty, text)
    {
    }

    public TextButton(Vector2 position, float scale, string text) : this(position, scale, string.Empty, text)
    {
    }

    public TextButton(Vector2 position, string name, string text) : this(position, 1F, name, text)
    {
    }

    public TextButton(Vector2 position, float scale, string name, string text) : this(position, DefaultSize * scale,
        name, text, DefaultTextScale)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text,  float textScale) :
        this(position, size, name, text, textScale, 1)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text,
        float textScale, int spacing) : this(position, size, name, text, textScale, spacing, DefaultTexture,
        DefaultMapping)
    {
    }

    public TextButton(Vector2 position, Vector2 size, string name, string text,
        float textScale, int spacing, Texture2D texture, TextureHitboxMapping mapping) :
        base(position, size, texture, mapping)
    {
        Text = new TextSystem.Text(text, Position, textScale, spacing);
        Text.Move(Rectangle.Center.ToVector2() - Text.Size / 2);
        Name = name;
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        UpdateRectangle();
        Text.Move(Rectangle.Center.ToVector2() - Text.Size / 2);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Text.Update(gameTime);
        Text.GetCalculator(Rectangle).
            OnCenter()
            .Centered()
            .Move();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        Text.ChangeColor(input);
    }

    public int ColorLength()
    {
        return Text.ColorLength();
    }
}