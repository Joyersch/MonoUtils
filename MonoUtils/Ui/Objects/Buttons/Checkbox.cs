using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons;

public class Checkbox : SquareTextButton
{
    private bool _checked;

    public Action<bool> ValueChanged;

    public Checkbox() : this(false)
    {
    }

    public Checkbox(float scale) : this(string.Empty, scale, false)
    {
    }

    public Checkbox(bool state) : this(string.Empty, state)
    {
    }

    public Checkbox(string text) : this(text, false)
    {
    }

    public Checkbox(float scale, bool state) : this(string.Empty, scale, state)
    {
    }

    public Checkbox(string text, float scale) : this(text, scale, false)
    {
    }

    public Checkbox(string text, bool state) : this(text, 1F, state)
    {
    }

    public Checkbox(string text, float scale, bool state) : base(text, scale, 2F)
    {
        _checked = state;
        Text.ChangeText(_checked ? "[checkmark]" : "[crossout]");
        Text.ChangeColor(new[] { _checked ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Red });
        Click += delegate
        {
            _checked = !_checked;
            Text.ChangeText(_checked ? "[checkmark]" : "[crossout]");
            Text.ChangeColor(new[]
                { _checked ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Red });
            ValueChanged?.Invoke(_checked);
        };
    }

    public bool Checked()
        => _checked;
}