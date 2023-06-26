namespace MonoUtils.Objects;

public static class CommandProcessor
{
    public static List<string> Process(string command)
    {
        if (command == "ping")
            return new[] {"pong"}.ToList();
        return new List<string>();
    } 
}