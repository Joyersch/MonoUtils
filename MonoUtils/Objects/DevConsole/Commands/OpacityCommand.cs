using Microsoft.Xna.Framework;

namespace MonoUtils.Objects.Commands;

public class OpacityCommand : ICommand
{
    [Command(Description = "Change opacity of the console", Name = "opacity")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length < 1)
            return new[] {"Usage:", "opacity [0..1]"};

        if (!float.TryParse(options[0].ToString(), out float value))
            return new[] {@$"Invalid value ""{value}"""};

        if (value > 1)
            return new[] {"Value to big"};

        if (value < 0)
            return new[] {"Value to small"};
        console.DrawColor = new Color(console.DrawColor, value);
        return new[] {"Changed opacity for console"};
    }
}