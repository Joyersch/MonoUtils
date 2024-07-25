using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logging;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui;

namespace MonoUtils.Ui.Logic;

public class MouseActionsMat : IMouseActions, IInteractable, IHitbox
{
    private readonly IHitbox _toCover;
    private readonly bool _sendSelfAsInvoker;
    private bool _hover;
    public bool IsHover => _hover;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;
    private bool _wasPressed;

    public MouseActionsMat(IHitbox toCover, bool sendSelfAsInvoker = false)
    {
        _toCover = toCover;
        _sendSelfAsInvoker = sendSelfAsInvoker;
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool isMouseHovering = false;
        foreach (Rectangle rectangle in toCheck.Hitbox)
            if (_toCover.Hitbox.Any(h => h.Intersects(rectangle)))
                isMouseHovering = true;

        bool isPressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
        if (isMouseHovering)
        {
            if (!_hover)
                Enter?.Invoke(_sendSelfAsInvoker ? this : _toCover);

            if (!_wasPressed && isPressed)
                Click?.Invoke(_sendSelfAsInvoker ? this : _toCover);
        }
        else if (_hover)
            Leave?.Invoke(_sendSelfAsInvoker ? this : _toCover);
        _hover = isMouseHovering;
        _wasPressed = isPressed;
    }

    public Rectangle[] Hitbox => _toCover.Hitbox;
}