using Microsoft.Xna.Framework;

namespace MonoUtils.Logic.Hitboxes.Collision;

public class HitboxCollection : List<IHitbox>, IHitbox
{
    public Rectangle[] Hitbox => this.SelectMany(c => c.Hitbox).ToArray();
}