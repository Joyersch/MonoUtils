using Microsoft.Xna.Framework;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.GameObjects;

public class EmptyHitbox : IHitbox
{
    public Rectangle[] Hitbox { get; } =  new Rectangle[0];
}