using MonoUtils.Objects;

namespace MonoUtils.Logging;

public class LogAdapter
{
    private TextWriter _writer;
    private DevConsole _console;

    private int _line;

    public LogAdapter(TextWriter writer)
    {
        _writer = writer;
    }

    public LogAdapter(DevConsole console)
    {
        _console = console;
    }

    public void SetLine(int line)
    {
        if (_writer is not null)
            throw new Exception("line not supported by TextWriter");
        _line = line;
    }

    public void Write(string text)
    {

        if (_writer is not null)
            _writer.Write(text);

        if (_console is not null)
            _console.Write(text, _line);
            
    }

    public void UpdateReference(DevConsole console)
    {
        _console = console;
        _writer = null;
    }
    
    public void UpdateReference(TextWriter writer)
    {
        _console = null;
        _writer = writer;
    }
}