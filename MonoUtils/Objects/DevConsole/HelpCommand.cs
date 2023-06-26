using MonoUtils.Logging;

namespace MonoUtils.Objects;

public class HelpCommand : ICommand
{
    [CommandAttribute(Description = "Shows all command and there description.", Name = "help")]
    public IEnumerable<string> Execute(object[] options)
        => CommandProcessor.Commands.Select(command => $"{command.Name} - {command.Description}");
}