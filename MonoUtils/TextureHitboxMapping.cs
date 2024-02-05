using Microsoft.Xna.Framework;

namespace MonoUtils;

public struct TextureHitboxMapping
{
    public Vector2 ImageSize;
    public Rectangle[] Hitboxes;
    public int AnimationFrames;
    public bool? AnimationFromTop;
    public float AnimationSpeed;
    public Vector2 Origin;
}