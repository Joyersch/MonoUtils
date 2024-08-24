using Microsoft.Xna.Framework;

namespace MonoUtils.Settings;

public sealed class Resolution
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Resolution(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return Width + "x" + Height;
    }

    public Vector2 ToVector2()
        => new Vector2(Width, Height);
}