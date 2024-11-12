namespace MonoUtils.Console.Commands;

public sealed class ClearCommand : ICommand
{
    [Command(Description = "Clears the console backlog.", Name = "clear")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        console.Backlog.Clear();
        return [];
    }
}