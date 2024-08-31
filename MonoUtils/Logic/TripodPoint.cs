using Microsoft.Xna.Framework;

namespace MonoUtils.Logic;

public struct TripodPoint(Vector2 position, float zoom)
{
    public Vector2 Position { get; set; } = position;
    public float Zoom { get; set; } = zoom;
}