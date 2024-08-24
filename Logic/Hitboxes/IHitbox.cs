using Microsoft.Xna.Framework;

namespace MonoUtils.Logic.Hitboxes;

public interface IHitbox
{
    public Rectangle[] Hitbox { get; }
}