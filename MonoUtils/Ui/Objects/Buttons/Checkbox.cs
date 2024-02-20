using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons;

public class Checkbox : TextButton<SquareButton>
{
    private bool _checked;

    public Action<bool> ValueChanged;

    public Checkbox() : this(false)
    {
    }

    public Checkbox(float scale) : this(scale, false)
    {
    }

    public Checkbox(bool state) : this(4F, state)
    {
    }

    public Checkbox(float scale, bool state) : base(string.Empty, new SquareButton(Vector2.Zero, scale))
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
        // Update Text
        Move(GetPosition());
    }

    public bool Checked()
        => _checked;
}