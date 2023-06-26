using System;

namespace MonoUtils.Logging;

public static class Log
{
    public static LogAdapter Out { get; set; }
    public static void WriteLine(string msg)
    {
        Out.Write(msg);
    }
}