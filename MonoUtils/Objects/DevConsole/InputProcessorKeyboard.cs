using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MonoUtils.Objects;

public static class InputProcessorKeyboard
{
    /// <summary>
    /// Use the GameWindow.TextInput event instead!
    /// </summary>
    /// <returns>the text of the currently pressed keys</returns>
    public static string GetPressedText()
    {
        var builder = new StringBuilder();

        for (Keys key = Keys.A; key <= Keys.Z; key++)
        {
            if (InputReaderKeyboard.CheckKey(key, true))
                builder.Append(ConvertKeysToLetterString(key));
        }

        return builder.ToString();
    }

    private static string ConvertKeysToLetterString(Keys key)
        => key switch
        {
            Keys.A => "a",
            Keys.B => "b",
            Keys.C => "c",
            Keys.D => "d",
            Keys.E => "e",
            Keys.F => "f",
            Keys.G => "g",
            Keys.H => "h",
            Keys.I => "i",
            Keys.J => "j",
            Keys.K => "k",
            Keys.L => "l",
            Keys.M => "m",
            Keys.N => "n",
            Keys.O => "o",
            Keys.P => "p",
            Keys.Q => "q",
            Keys.R => "r",
            Keys.S => "s",
            Keys.T => "t",
            Keys.U => "u",
            Keys.V => "v",
            Keys.W => "w",
            Keys.X => "x",
            Keys.Y => "y",
            Keys.Z => "z",
            Keys.Space => "",
            _ => ""
        };
}