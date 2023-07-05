namespace MonoUtils.Ui.Objects.Console.Commands;

public class HelpCommand : ICommand
{
    [CommandAttribute(Description = "Shows all command and there description.", Name = "help")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
        => console.Processor.Commands.Select(command => $"{command.Name} - {command.Description}");
}