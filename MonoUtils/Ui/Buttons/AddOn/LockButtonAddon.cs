using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.GameObjects;
using MonoUtils.Helper;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Ui.Buttons.AddOn;

public sealed class LockButtonAddon : ButtonAddon
{
    public bool IsLocked { get; private set; } = true;

    private readonly Text _text;
    private Microsoft.Xna.Framework.Color _savedButtonColor;

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
        _savedButtonColor = button.GetColor()[0];
        UpdateText();
        Lock();
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
        Button.ChangeColor(new[] { _savedButtonColor });
    }

    public void Lock()
    {
        IsLocked = true;
        UpdateText();
        var color = ColorHelper.DarkenColor(_savedButtonColor, 0.87F);
        Button.ChangeColor(new[] { color });
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

    public override void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        base.ChangeColor(input);
        _savedButtonColor = input[0];
    }
}