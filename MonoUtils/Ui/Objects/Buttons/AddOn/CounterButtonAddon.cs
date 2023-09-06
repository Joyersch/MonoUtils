using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.TextSystem;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public class CounterButtonAddon : ButtonAddonBase
{
    private int _states;
    private readonly Text _text;

    public CounterButtonAddon(ButtonAddonAdapter button, int startStates, float scale = 1F) : base(button, scale)
    {
        _states = startStates;
        _text = new Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(),
            Position, Scale);
        Size = _text.Rectangle.Size.ToVector2();
        Button.SetIndicatorOffset((int) Size.X);
        UpdateText();
    }

    public override Rectangle GetRectangle()
        => Button.GetRectangle();

    public override void SetDrawColor(Microsoft.Xna.Framework.Color color)
        => Button.SetDrawColor(color);


    public override void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        Button.UpdateInteraction(gameTime, toCheck);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text.Update(gameTime);
        Button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Button.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    private void UpdateText()
    {
        _text.ChangeText(_states.ToString());
    }

    protected override void ButtonCallback(object obj, IButtonAddon.CallState state)
    {
        if (state == IButtonAddon.CallState.Click && _states > 0)
            _states--;

        if (_states == 0)
        {
            base.ButtonCallback(obj, state);
            _text.ChangeText(string.Empty);
        }

        UpdateText();
    }

    public override Vector2 GetPosition()
        => Button.GetPosition();

    public override Vector2 GetSize()
        => Button.GetSize();

    public override void Move(Vector2 newPosition)
    {
        Button.Move(newPosition);
        _text.Move(newPosition);
        Position = newPosition;
    }
    
    public override void MoveIndicatorBy(Vector2 newPosition)
    {
        _text.Move(_text.Position + newPosition);
        Position += newPosition;
    }
}