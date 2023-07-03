namespace MonoUtils.Ui.Objects.Console.Commands;

public class OpacityCommand : ICommand
{
    [Command(Description = "Change opacity of the console", Name = "opacity")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length < 1)
            return new[] {"Usage:", "opacity [0..1]"};

        if (!float.TryParse(options[0].ToString(), out float value))
            return new[] {@$"Invalid value ""{value}"""};

        console.DrawColor = new Microsoft.Xna.Framework.Color(value, value, value, value);
        return new[] {"Changed opacity for console"};
    }
}