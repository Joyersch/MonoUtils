namespace MonoUtils.Console;

public interface IProcessor
{
    public IEnumerable<string> Process(DevConsole caller, string fullCommand, ContextProvider context);
}