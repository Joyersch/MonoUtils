using Microsoft.Xna.Framework;

namespace MonoUtils.Logic;

public static class SpartialExtensions
{
    public static Rectangle GetRectangle(this ISpatial spatial)
        => new Rectangle(spatial.GetPosition().ToPoint(), spatial.GetSize().ToPoint());
}