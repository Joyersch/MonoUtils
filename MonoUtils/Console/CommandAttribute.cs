namespace MonoUtils.Console;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Name { get; set; }
    public string Description { get; set; }
}