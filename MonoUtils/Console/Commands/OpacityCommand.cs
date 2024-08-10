namespace MonoUtils.Console.Commands;

public sealed class OpacityCommand : ICommand
{
    private Microsoft.Xna.Framework.Color? _color;

    [Command(Description = "Change opacity of the console", Name = "opacity")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length < 1)
            return new[] { "Usage:", "opacity (0..1)" };

        if (options[0].ToString() is null)
            return new[] { "Invalid input supplied!" };

        if (!float.TryParse(options[0].ToString()!.Replace(',', '.'), out float value))
            return new[] { @$"Invalid value ""{value}""" };

        if (value > 1F)
            value = 1F;

        _color ??= console.GetColor()[0];
        Microsoft.Xna.Framework.Color color = console.GetColor()[0];
        color.R = (byte)(value * _color.Value.R);
        color.G = (byte)(value * _color.Value.G);
        color.B = (byte)(value * _color.Value.B);
        color.A = (byte)(value * _color.Value.A);
        console.ChangeColor(new[] { color });
        return new[] { "Changed opacity for console" };
    }
}