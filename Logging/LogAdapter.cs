using Microsoft.Xna.Framework;
using MonoUtils.Console;

namespace MonoUtils.Logging;

public sealed class LogAdapter
{
    private TextWriter? _writer;
    private DevConsole? _console;

    private int _line;

    public bool IsConsole => _console is not null;
    public bool IsWriter => _writer is not null;

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
            _writer.Write(text + Environment.NewLine);

        if (_console is not null)
            _console.Write(text, _line);
    }

    public void WriteColor(string text, Color[] color)
    {
        if (_console is not null)
            _console.WriteColor(text, new BacklogColorSet(color));
        if (_writer is not null)
            _writer.Write(text + Environment.NewLine);
    }

    public void WriteColor(string text, Color color)
    {
        if (_console is not null)
            _console.WriteColor(text, new BacklogColorSet(color, text.Length));
        if (_writer is not null)
            _writer.Write(text + Environment.NewLine);
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