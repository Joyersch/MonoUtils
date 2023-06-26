namespace MonoUtils.Objects;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Name { get; set; }
    public string Description { get; set; }
}