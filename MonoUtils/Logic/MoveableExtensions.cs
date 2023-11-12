namespace MonoUtils.Logic;

public static class MoveableExtension
{
    public static PositionCalculator GetCalculator(this IMoveable sender, Microsoft.Xna.Framework.Rectangle rectangle)
        => new (rectangle, sender);
    
    public static PositionCalculator GetCalculator(this IMoveable sender, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size)
        => new (new Microsoft.Xna.Framework.Rectangle(position.ToPoint(), size.ToPoint()), sender);

    public static AnchorCalculator GetAnchor(this IMoveable sender, IMoveable receiver)
        => new(sender, receiver);

}