using Microsoft.Xna.Framework;

namespace MonoUtils.Ui;

public struct TextureHitboxMapping
{
    public Vector2 ImageSize;
    public Rectangle[] Hitboxes;
    public int AnimationsFrames;
    public bool? AnimationFromTop;
}