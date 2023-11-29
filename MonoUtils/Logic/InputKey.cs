using Microsoft.Xna.Framework.Input;

namespace MonoUtils.Logic;

public class InputKey
{
    private InputReaderMouse.MouseKeys _mouseKey;
    private Keys _keyboardKey;

    public InputKey(InputReaderMouse.MouseKeys key)
    {
        _mouseKey = key;
        _keyboardKey = Keys.None;
    }

    public InputKey(Keys key)
    {
        _mouseKey = InputReaderMouse.MouseKeys.None;
        _keyboardKey = key;
    }

    public void GetKey(out Keys keyboardKey, out InputReaderMouse.MouseKeys mouseKey)
    {
        keyboardKey = _keyboardKey;
        mouseKey = _mouseKey;
    }
}