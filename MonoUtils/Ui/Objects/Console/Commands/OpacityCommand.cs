namespace MonoUtils.Ui.Objects.Console.Commands;

public class OpacityCommand : ICommand
{
    [Command(Description = "Change opacity of the console", Name = "opacity")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length < 1)
            return new[] { "Usage:", "opacity [0..1]" };

        if (!float.TryParse(options[0].ToString().Replace('.', ','), out float value))
            return new[] { @$"Invalid value ""{value}""" };

        _color ??= console.DrawColor;
        console.DrawColor.R = (byte)(value * _color.Value.R);
        console.DrawColor.G = (byte)(value * _color.Value.G);
        console.DrawColor.B = (byte)(value * _color.Value.B);
        console.DrawColor.A = (byte)(value * _color.Value.A);
        return new[] { "Changed opacity for console" };
    }

    private Microsoft.Xna.Framework.Color? _color = null;
}