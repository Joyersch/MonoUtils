using MonoUtils.Objects;

namespace MonoUtils.Logging;

public class LogAdapter
{
    private readonly TextWriter _writer;
    private readonly DevConsole _console;

    public LogAdapter(TextWriter writer)
    {
        _writer = writer;
    }

    public LogAdapter(DevConsole console)
    {
        _console = console;
    }

    public void Write(string text)
    {
        if (_writer is not null)
            _writer.Write(text);
        if (_console is not null)
            _console.Write(text);
    }
}