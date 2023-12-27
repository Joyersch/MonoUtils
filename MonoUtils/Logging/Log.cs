using System;
using Microsoft.Xna.Framework;

namespace MonoUtils.Logging;

public static class Log
{
    public static LogAdapter Out { get; set; }

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

    public static void WriteError(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"[SBO]Error[SBC] {msg}", Color.Red);
    }

    public static void WriteCritical(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"[SBO]Critical[SBC] {msg}", Color.DarkRed);
    }

    public static void WriteWarning(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"[SBO]Warning[SBC] {msg}", Color.Gold);
    }

    public static void WriteInformation(string msg)
    {
        Out.SetLine(-1);
        Out.WriteColor($"[SBO]Info[SBC] {msg}", Color.DeepSkyBlue);
    }
}