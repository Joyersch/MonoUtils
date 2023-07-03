using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.GameObjects;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public class LockButtonAddon : ButtonAddonBase
{
    public bool IsLocked { get; private set; } = true;

    private readonly ButtonAddonAdapter _button;
    private readonly Text _text;

    public LockButtonAddon(ButtonAddonAdapter button) : base(button)
    {
        _button = button;
        _text = new Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(), Position);
        
        // Button is locked by default
        _button.SetDrawColor(Microsoft.Xna.Framework.Color.DarkGray);
        
        UpdateText();
        Size = _text.Size;
        _button.SetIndicatorOffset((int) Size.X);
    }

    public override void SetIndicatorOffset(int x)
    {
        _button.SetIndicatorOffset(x);
    }

    protected override void ButtonCallback(object sender, IButtonAddon.CallState state)
    {
        if (IsLocked && state == IButtonAddon.CallState.Click)
            return;

        base.ButtonCallback(sender, state);
    }

    public override void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        if (!IsLocked)
            _button.UpdateInteraction(gameTime, toCheck);
        else
            _button.UpdateInteraction(gameTime, new EmptyHitbox());
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text.Update(gameTime);
        _button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    public void Unlock()
    {
        IsLocked = false;
        _button.SetDrawColor(Microsoft.Xna.Framework.Color.White);
        UpdateText();
    }

    public void Lock()
    {
        IsLocked = true;
        _button.SetDrawColor(Microsoft.Xna.Framework.Color.DarkGray);
        UpdateText();
    }

    private void UpdateText()
    {
        _text.ChangeText(IsLocked
            ? Letter.ReverseParse(Letter.Character.LockLocked).ToString()
            : Letter.ReverseParse(Letter.Character.LockUnlocked).ToString());
        _text.ChangeColor(IsLocked ? Microsoft.Xna.Framework.Color.Gray : Microsoft.Xna.Framework.Color.DarkGray);
    }

    public override Vector2 GetPosition()
        => _button.GetPosition();

    public override Vector2 GetSize()
        => _button.GetSize();

    public override Rectangle GetRectangle()
        => _button.GetRectangle();

    public override void SetDrawColor(Microsoft.Xna.Framework.Color color)
        => _button.SetDrawColor(color);

    public override void Move(Vector2 newPosition)
    {
        _button.Move(newPosition);
        _text.Move(newPosition);
        Position = newPosition;
    }

    public override void MoveIndicatorBy(Vector2 newPosition)
    {
        _text.Move(_text.Position + newPosition);
        Position += newPosition;
    }
}