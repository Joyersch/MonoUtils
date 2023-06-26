namespace MonoUtils.Objects;

public interface ICommand
{
    public IEnumerable<string> Execute(object[] options);
}