using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.GameObjects;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public class LockButtonAddon : ButtonAddonBase
{
    public bool IsLocked { get; private set; } = true;

    private readonly Text _text;

    public LockButtonAddon(ButtonAddonAdapter button, float scale = 1F) : base(button, scale)
    {
        _text = new Text("[locklocked]", Position, Scale);
        
        // Button is locked by default
        Button.SetDrawColor(Microsoft.Xna.Framework.Color.DarkGray);
        
        UpdateText();
        Size = _text.Size;
        Button.SetIndicatorOffset((int) Size.X);
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
            Button.UpdateInteraction(gameTime, toCheck);
        else
            Button.UpdateInteraction(gameTime, new EmptyHitbox());
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

    public void Unlock()
    {
        IsLocked = false;
        Button.SetDrawColor(Microsoft.Xna.Framework.Color.White);
        UpdateText();
    }

    public void Lock()
    {
        IsLocked = true;
        Button.SetDrawColor(Microsoft.Xna.Framework.Color.DarkGray);
        UpdateText();
    }

    private void UpdateText()
    {
        _text.ChangeText(IsLocked
            ? "[locklocked]"
            : "[lockunlocked]");
        _text.ChangeColor(IsLocked ? Microsoft.Xna.Framework.Color.Gray : Microsoft.Xna.Framework.Color.DarkGray);
    }

    public override Vector2 GetPosition()
        => Button.GetPosition();

    public override Vector2 GetSize()
        => Button.GetSize();

    public override Rectangle GetRectangle()
        => Button.GetRectangle();

    public override void SetDrawColor(Microsoft.Xna.Framework.Color color)
        => Button.SetDrawColor(color);

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