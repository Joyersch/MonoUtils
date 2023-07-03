using Microsoft.Xna.Framework;
using MonoUtils.Ui;

namespace MonoUtils.Ui.Logic;

public static class ScreenMatrix
{
    public static Rectangle Screen(int x, int y, float d)
        => new(new Point((int) (Display.CustomWidth * x / d), (int) (Display.CustomHeight * y / d)),
            (Display.Size / d).ToPoint());
}