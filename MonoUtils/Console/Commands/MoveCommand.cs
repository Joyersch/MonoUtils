using Microsoft.Xna.Framework;

namespace MonoUtils.Ui.Objects.Console.Commands;

public class MoveCommand : ICommand
{
    [CommandAttribute(Description = "move console to location", Name = "move")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        if (options.Length < 2)
            return new[] {"Usage:", "move (x) (y)"};

        if (!int.TryParse(options[0].ToString(), out int x))
            return new[] {"Bad value for x"};

        if (!int.TryParse(options[1].ToString(), out int y))
            return new[] {"Bad value for y"};

        caller.Move(new Vector2(x, y));
        return new[] {"moved console!"};
    }
}