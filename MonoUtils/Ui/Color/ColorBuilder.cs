namespace MonoUtils.Ui.Color;

public class ColorBuilder
{
    private List<Microsoft.Xna.Framework.Color> _colors = new();

    public void AddColor(Microsoft.Xna.Framework.Color color, int length)
    {
        for (int i = 0; i < length; i++)
            _colors.Add(color);
    }

    public Microsoft.Xna.Framework.Color[] GetColor()
        => _colors.ToArray();

    public void AddColor(Microsoft.Xna.Framework.Color[] color)
        => _colors.AddRange(color);
}