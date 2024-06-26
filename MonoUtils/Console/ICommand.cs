namespace MonoUtils.Console;

public interface ICommand
{
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context);
}