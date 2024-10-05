using Microsoft.Xna.Framework;
using MonoUtils.Logic.Management;

namespace MonoUtils.Logic.Positioning;

public class OffsetSizePositionJob : PositionCalculatorJob
{
    private readonly IMoveable _moveable;
    private readonly float _percent;
    private readonly bool _onlyX;
    private readonly bool _onlyY;

    public OffsetSizePositionJob(IMoveable moveable, float percent, bool onlyX = false, bool onlyY = false)
    {
        _moveable = moveable;
        _percent = percent;
        _onlyX = onlyX;
        _onlyY = onlyY;
    }

    public override Vector2 Execute(Rectangle area, Vector2 prior)
    {
        var bySize = _moveable.GetSize() * _percent;

        if (_onlyX) return new Vector2(prior.X + bySize.X, prior.Y);
        if (_onlyX) return new Vector2(prior.X, prior.Y + bySize.Y);
        return prior + bySize;
    }
}