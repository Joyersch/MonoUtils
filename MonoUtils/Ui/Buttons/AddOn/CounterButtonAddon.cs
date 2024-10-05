using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Ui.Buttons.AddOn;

public sealed class CounterButtonAddon : ButtonAddon
{
    private int _states;
    private readonly Text _text;

    public CounterButtonAddon(IButton button, int startStates, float scale = 1F) : base(button)
    {
        _states = startStates;
        _text = new Text("", GetPosition(), scale * Text.DefaultLetterScale);
        UpdateText();
        Button.Click += delegate
        {
            if (_states != 0)
                _states--;

            UpdateText();

            if (_states == 0)
                InvokeClick();
        };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    private void UpdateText()
    {
        _text.ChangeText(_states.ToString());
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        _text.Move(newPosition);
    }

    public override void SetScale(float scale)
    {
        base.SetScale(scale);
        _text.SetScale(scale);
    }
}