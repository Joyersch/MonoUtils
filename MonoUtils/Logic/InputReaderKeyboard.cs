﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoUtils;

public static class InputReaderKeyboard
{
    private static Dictionary<Keys, bool> _currentlyPressedKeys = new();
    private static bool _anyKeyPressed;

    /// <summary>
    /// Check if searched key is being pressed.
    /// </summary>
    /// <param name="search"></param>
    /// <param name="onlyOnces"> if true, stores searched key if pressed. Otherwise search is not stored</param>
    /// <returns>Returns if search is being pressed. If <paramref name="onlyOnces"/> is true, only returns true if search is not stored./></returns>
    public static bool CheckKey(Keys search, bool onlyOnces = false)
    {
        var keyboardState = Keyboard.GetState();
        var isKeyDown = keyboardState.IsKeyDown(search);

        if (!onlyOnces)
            return isKeyDown;

        if (!_currentlyPressedKeys.ContainsKey(search))
            _currentlyPressedKeys.Add(search, isKeyDown);

        var returnValue = !_currentlyPressedKeys[search] && isKeyDown;
        _currentlyPressedKeys[search] = isKeyDown;

        return returnValue;
    }

    /// <summary>
    /// check if any key is pressed.
    /// </summary>
    /// <param name="onlyOnces"></param>
    /// <returns>true if any key is pressed, else false</returns>
    public static bool AnyKeyPress(bool onlyOnces)
    {
        var x = Keyboard.GetState().GetPressedKeys();
        if (x.Length == 0)
            return _anyKeyPressed = false;

        if (_anyKeyPressed)
            return false;

        if (!onlyOnces)
            return _anyKeyPressed = true;

        return true;
    }

    public static Dictionary<Keys, bool> GetCurrentKeys()
        => _currentlyPressedKeys;
}