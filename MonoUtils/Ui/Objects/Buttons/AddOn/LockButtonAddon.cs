using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.GameObjects;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public class LockButtonAddon : ButtonAddon
{
    public bool IsLocked { get; private set; } = true;

    private readonly Text _text;

    public LockButtonAddon(IButton button, float scale = 1F) : base(button)
    {
        _text = new Text("[locklocked]", GetPosition(), scale);

        button.Enter += _ => InvokeEnter();
        button.Click += delegate
        {
            if (!IsLocked)
                InvokeClick();
        };
        button.Leave += _ => InvokeLeave();
        UpdateText();
    }

    public override void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        Button.UpdateInteraction(gameTime, !IsLocked ? toCheck : new EmptyHitbox());
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text.Update(gameTime);
        Button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    public void Unlock()
    {
        IsLocked = false;
        UpdateText();
    }

    public void Lock()
    {
        IsLocked = true;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.ChangeText(IsLocked
            ? "[locklocked]"
            : "[lockunlocked]");
        _text.ChangeColor(IsLocked ? Microsoft.Xna.Framework.Color.Gray : Microsoft.Xna.Framework.Color.DarkGray);
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        _text.Move(newPosition);
    }
}