using Microsoft.Xna.Framework.Input;

namespace MonoUtils.Logic;

public static class InputReader
{
    public static bool CheckKey(InputKey search, bool onlyOnces = false)
    {
        search.GetKey(out var keyboard, out var mouse);

        if (keyboard != Keys.None)
            return InputReaderKeyboard.CheckKey(keyboard, onlyOnces);

        if (mouse != InputReaderMouse.MouseKeys.None)
            return InputReaderMouse.CheckKey(mouse, onlyOnces);

        return false;
    }

    public static void GetAPressedKey(out Keys keyboardKey, out InputReaderMouse.MouseKeys mouseKey)
    {
        keyboardKey = Keyboard.GetState().GetPressedKeys().FirstOrDefault(Keys.None);
        mouseKey = InputReaderMouse.MouseKeys.None;

        var mouseState = Mouse.GetState();
        var scrollDifference = InputReaderMouse.ScrollWheelValue - mouseState.ScrollWheelValue;

        if (mouseState.LeftButton == ButtonState.Pressed)
            mouseKey = InputReaderMouse.MouseKeys.Left;

        if (mouseState.MiddleButton == ButtonState.Pressed)
            mouseKey = InputReaderMouse.MouseKeys.Middle;

        if (mouseState.RightButton == ButtonState.Pressed)
            mouseKey = InputReaderMouse.MouseKeys.Right;

        mouseKey = scrollDifference switch
        {
            < 0 => InputReaderMouse.MouseKeys.MouseUp,
            > 0 => InputReaderMouse.MouseKeys.MouseDown,
            _ => mouseKey
        };
    }
}