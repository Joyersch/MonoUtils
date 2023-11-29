using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logging;

namespace MonoUtils;

public static class InputReaderMouse
{
    public enum MouseKeys
    {
        None = Keys.OemClear + 1000,
        Left,
        Middle,
        Right,
        MouseUp,
        MouseDown
    }

    private static Dictionary<MouseKeys, ButtonState> _storedMouseStates = new()
    {
        { MouseKeys.Left, ButtonState.Released },
        { MouseKeys.Middle, ButtonState.Released },
        { MouseKeys.Right, ButtonState.Released },
        { MouseKeys.MouseUp, ButtonState.Released },
        { MouseKeys.MouseDown, ButtonState.Released },
    };

    public static int ScrollWheelValue => _scrollWheelValue;

    private static int _scrollWheelValue;

    /// <summary>
    /// Stores the current key-states. Call this at the end of update
    /// </summary>
    public static void StoreButtonStates()
    {
        MouseState mouseState = Mouse.GetState();
        _storedMouseStates[MouseKeys.Left] = mouseState.LeftButton;
        _storedMouseStates[MouseKeys.Middle] = mouseState.MiddleButton;
        _storedMouseStates[MouseKeys.Right] = mouseState.RightButton;
        var scrollDifference = _scrollWheelValue - mouseState.ScrollWheelValue;
        _storedMouseStates[MouseKeys.MouseUp] = scrollDifference < 0 ? ButtonState.Pressed : ButtonState.Released;
        _storedMouseStates[MouseKeys.MouseDown] = scrollDifference > 0 ? ButtonState.Pressed : ButtonState.Released;
        _scrollWheelValue = mouseState.ScrollWheelValue;
    }

    /// <summary>
    /// Check if a key is pressed.
    /// </summary>
    /// <param name="search">Key to be checked</param>
    /// <param name="onlyOnces">If true, will return false if stored button states contain search</param>
    /// <returns>if <paramref name="search"/> is beeing pressed, returns true else false. </returns>
    public static bool CheckKey(MouseKeys search, bool onlyOnces = true)
    {
        var mouseState = Mouse.GetState();
        var scrollDifference = _scrollWheelValue - mouseState.ScrollWheelValue;
        return !(onlyOnces && _storedMouseStates.Any(s => s.Key == search && s.Value == ButtonState.Pressed))
               && search switch
               {
                   MouseKeys.Left => mouseState.LeftButton == ButtonState.Pressed,
                   MouseKeys.Middle => mouseState.MiddleButton == ButtonState.Pressed,
                   MouseKeys.Right => mouseState.RightButton == ButtonState.Pressed,
                   MouseKeys.MouseUp => scrollDifference < 0,
                   MouseKeys.MouseDown => scrollDifference > 0,
                   _ => false
               };
    }
}