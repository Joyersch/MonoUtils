using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;

namespace MonoUtils.Ui.Buttons;

public interface IButton : IHitbox, IManageable, IMoveable, IColorable, IMouseActions, IInteractable, ILayerable, IScaleable
{
    public bool IsHover { get; }
}