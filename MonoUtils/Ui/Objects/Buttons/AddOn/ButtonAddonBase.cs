using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public abstract class ButtonAddonBase : GameObject, IInteractable, IMoveable, IButtonAddon
{
    public event Action<object, IButtonAddon.CallState> Callback;
    protected readonly ButtonAddonAdapter Button;
    protected readonly float Scale;

    public ButtonAddonBase(ButtonAddonAdapter button, float scale) : base(button.Position, button.Size, DefaultTexture,
        DefaultMapping)
    {
        Button = button;
        Scale = scale;
        button.Callback += ButtonCallback;
    }

    protected virtual void ButtonCallback(object sender, IButtonAddon.CallState state)
    {
        Callback?.Invoke(sender, state);
    }

    public abstract Rectangle GetRectangle();

    public abstract void UpdateInteraction(GameTime gameTime, IHitbox toCheck);

    public virtual void SetIndicatorOffset(int x)
    {
        Button.SetIndicatorOffset(x);
    }

    public abstract void SetDrawColor(Microsoft.Xna.Framework.Color color);

    public override Vector2 GetPosition()
        => Vector2.Zero;

    public override Vector2 GetSize()
        => Vector2.Zero;

    public override void Move(Vector2 newPosition)
    {
    }

    public abstract void MoveIndicatorBy(Vector2 newPosition);

    public virtual bool IsHover() => Button.IsHover();
}