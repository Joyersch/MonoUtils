using System;

namespace MonoUtils.Logging;

public static class Log
{
    public static LogAdapter Out { get; set; }
    public static void Write(string msg)
    {
        Out.Write(msg);
    }
    
    public static void WriteLine(string msg, int line)
    {
        Out.SetLine(line);
        Out.Write(msg);
    }
}