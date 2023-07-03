using Microsoft.Xna.Framework;
using MonoUtils.Ui.Color;

namespace MonoUtils.Ui.Logic.Listener;

public class ColorListener
{
    private readonly List<(AnimatedColor color, IColorable colorable)> _mappings;

    public ColorListener()
    {
        _mappings = new List<(AnimatedColor color, IColorable colorable)>();
    }

    public void Update(GameTime gameTime)
        => _mappings.ForEach(m => m.colorable.ChangeColor(m.color.GetColor(m.colorable.ColorLength())));

    public void Add(AnimatedColor color, IColorable text)
        => _mappings.Add((color, text));
}