using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public abstract class ButtonAddonBase : GameObject, IInteractable, IMoveable, IButtonAddon
{
    public event Action<object, IButtonAddon.CallState> Callback;

    public ButtonAddonBase(ButtonAddonAdapter button) : base(button.Position, button.Size, DefaultTexture, DefaultMapping)
    {
        button.Callback += ButtonCallback;
    }

    protected virtual void ButtonCallback(object sender, IButtonAddon.CallState state)
    {
        Callback?.Invoke(sender,state);
    }

    public abstract Rectangle GetRectangle();

    public abstract void UpdateInteraction(GameTime gameTime, IHitbox toCheck);

    public abstract void SetIndicatorOffset(int x);

    public abstract void SetDrawColor(Microsoft.Xna.Framework.Color color);

    public abstract Vector2 GetPosition();

    public abstract Vector2 GetSize();

    public abstract void Move(Vector2 newPosition);

    public abstract void MoveIndicatorBy(Vector2 newPosition);
}