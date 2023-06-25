namespace MonoUtils.Objects;

public static class CommandProcessor
{
    public static List<string> Process(string command)
    {
        return new[] {command}.ToList();
    } 
}