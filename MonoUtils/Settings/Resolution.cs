using Microsoft.Xna.Framework;
using MonoUtils.Logic;

namespace MonoUtils.Settings;

public class Resolution : IChangeable
{
    private int _width;
    public int Width
    {
        get => _width;
        set
        {
            _width = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _height;
    public int Height
    {
        get => _height;
        set
        {
            _height = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler HasChanged;

    public Vector2 ToVector2() => new(_width, _height);
}