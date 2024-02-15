using Microsoft.Xna.Framework;

namespace MonoUtils.Logic;

public interface IRotateable
{
    public float Rotation { get; set; }

    public Vector2 GetPosition();

    public Vector2 GetSize();
}