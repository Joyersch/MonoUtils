using System;
using Microsoft.Xna.Framework;

namespace MonoUtils.Logging;

public static class Log
{
    public static LogAdapter Out { get; set; }
    private static string leftBracket => Out.IsConsole ? "[SBO]" : "[";
    private static string rightBracket => Out.IsConsole ? "[SBC]" : "]";

    public static void Write(string msg)
    {
        Out.SetLine(-1);
        Out.Write(msg);
    }

    public static void WriteLine(string msg, int line)
    {
        Out.SetLine(line);
        Out.Write(msg);
    }

    public static void WriteColor(string msg, Color[] colors)
    {
        Out.SetLine(-1);
        Out.WriteColor(msg, colors);
    }

    public static void Error(string msg)
    {
        Out.SetLine(-1);

        Out.WriteColor($"{leftBracket}Error{rightBracket} {msg}", Color.Red);
    }

    public static void Critical(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"{leftBracket}Critical{rightBracket} {msg}", Color.DarkRed);
    }

    public static void Warning(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"{leftBracket}Warning{rightBracket} {msg}", Color.Gold);
    }

    public static void Information(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"{leftBracket}Info{rightBracket} {msg}", Color.DeepSkyBlue);
    }
}