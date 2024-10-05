using Microsoft.Xna.Framework;

namespace MonoUtils.Logic.Positioning;

public abstract class PositionCalculatorJob
{
    public abstract Vector2 Execute(Rectangle area, Vector2 prior);
}