using Microsoft.Xna.Framework;

namespace MonoUtils.Ui.Buttons;

public sealed class SquareTextButton : TextButton<SquareButton>
{
    public static float DefaultScale { get; set; } = 4F;

    public SquareTextButton(string text) : this(text, Vector2.Zero)
    {
    }

    public SquareTextButton(string text, Vector2 position) : this(text, position, DefaultScale)
    {
    }

    public SquareTextButton(string text, Vector2 position, float scale) : base(text, new SquareButton(position, scale))
    {
    }
}