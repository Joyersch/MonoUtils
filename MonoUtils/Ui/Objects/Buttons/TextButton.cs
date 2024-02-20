using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons;

public class TextButton<T> : IButton where T : IButton
{
    public Text Text { get; }

    private T _button;
    public Rectangle[] Hitbox => _button.Hitbox;
    public Rectangle Rectangle => _button.Rectangle;
    public event Action<object>? Leave;
    public event Action<object>? Enter;
    public event Action<object>? Click;

    public TextButton(string text, T button) : this(text, 1F, button)
    {
    }

    public TextButton(string text, float scale, T button)
    {
        _button = button;
        _button.Leave += _ => Leave?.Invoke(this);
        _button.Enter += _ => Enter?.Invoke(this);
        _button.Click +=_ => Click?.Invoke(this);
        Text = new Text(text, scale);
        Text.GetCalculator(_button.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
    }

    public float Layer
    {
        get => _button.Layer;
        set => _button.Layer = value;
    }

    public bool IsHover => _button.IsHover;

    public void Update(GameTime gameTime)
    {
        _button.Update(gameTime);
        Text.Update(gameTime);
        Text.GetCalculator(_button.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        Text.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _button.GetPosition();

    public Vector2 GetSize()
        => _button.GetSize();

    public void Move(Vector2 newPosition)
    {
        _button.Move(newPosition);
        Text.GetCalculator(_button.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => Text.ChangeColor(input);

    public int ColorLength()
        => Text.ColorLength();

    public Microsoft.Xna.Framework.Color[] GetColor()
        => Text.GetColor();
}