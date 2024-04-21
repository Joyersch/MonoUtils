namespace MonoUtils.Ui.Objects.Console;

public interface IProcessor
{
    public IEnumerable<string> Process(DevConsole caller, string fullCommand, ContextProvider context);
}