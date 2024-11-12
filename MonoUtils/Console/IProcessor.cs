namespace MonoUtils.Console;

public interface IProcessor
{
    public List<(CommandAttribute Attribute, CommandOptionsAttribute[] Options, ICommand Command)> Commands { get; }

    public IEnumerable<string> Process(DevConsole caller, string fullCommand, ContextProvider context);

    public string? PossibleMatch(string search);
}