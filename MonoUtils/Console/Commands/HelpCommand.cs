namespace MonoUtils.Console.Commands;

public sealed class HelpCommand : ICommand
{
    [Command(Description = "Shows all command and there description.", Name = "help")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
        => console.Processor.Commands.Select(command => $"{command.Name} - {command.Description}");
}