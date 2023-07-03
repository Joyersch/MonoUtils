using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Ui.Objects;

/// <summary>
/// Not Tested. Use with caution!
/// </summary>
public class DragableObject : GameObject, IInteractable
{
    private bool _hover;
    private bool _isDrag;
    private Vector2 oldInterableLocation;

    public DragableObject(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool isMouseHovering = false;
        foreach (Rectangle rectangle in toCheck.Hitbox)
            if (HitboxCheck(rectangle))
                isMouseHovering = true;
        
        Rectangle rec = toCheck.Hitbox[0];
        rec = toCheck.Hitbox.Aggregate(rec, Rectangle.Union);

        var newInteractableLocation = rec.Center.ToVector2();
        if (_isDrag)
            Move(Position + oldInterableLocation - newInteractableLocation);
        
        if (isMouseHovering)
            _isDrag = InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false);
        Log.WriteLine(newInteractableLocation.ToString(), 0);
        oldInterableLocation = newInteractableLocation;
    }
}