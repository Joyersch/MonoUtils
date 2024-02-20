using Microsoft.Xna.Framework;

namespace MonoUtils.Logic;

public interface ISpatial
{
    public Vector2 GetPosition();
    public Vector2 GetSize();
}