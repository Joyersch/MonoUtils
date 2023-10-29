using Microsoft.Xna.Framework;
using MonoUtils.Ui.Objects.Console;

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
            return;
        
        _line = line;
    }

    public void Write(string text)
    {

        if (_writer is not null)
            _writer.Write(text);

        if (_console is not null)
            _console.Write(text, _line);
            
    }

    public void WriteColor(string text, Color[] color)
    {
        if (_writer is not null)
            return;

        if (_console is null)
            return;

        _console.WriteColor(text, new BacklogColorSet(color));
    }

    public void WriteColor(string text, Color color)
    {
        if (_writer is not null)
            return;

        if (_console is null)
            return;

        _console.WriteColor(text, new BacklogColorSet(color, text.Length));
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