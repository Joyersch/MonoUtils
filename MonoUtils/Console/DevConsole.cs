using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Console;

public sealed class DevConsole : IManageable, ILayerable, IColorable
{
    private Color _color;

    private Text _currentInput;
    private Text _cursorDisplay;
    private Text _maxText;
    public Backlog Backlog { get; private set; }
    private List<BacklogRow> _toDisplay;
    private int _maxLinesY;
    private bool _isDrawingCursor;
    private OverTimeInvoker _drawCursorInvoker;
    private Blank _background;

    public CommandProcessor Processor { get; private set; }

    public ContextProvider Context { get; private set; }

    private Text[] _lines;

    public Rectangle Rectangle => _scene.Camera.Rectangle;

    private Scene _scene;

    public float Layer { get; set; }

    public DevConsole(CommandProcessor processor, Scene scene) : this(processor,
        scene, null)
    {
    }

    public DevConsole(CommandProcessor processor, Scene scene, DevConsole? console)
    {
        Processor = console is null ? processor : console.Processor;
        _scene = scene;
        _maxText = new Text("[block]", scene.Display.Scale * 4);
        _currentInput = new Text(string.Empty, scene.Display.Scale * 4);

        Backlog = console is null ? new Backlog() : console.Backlog;

        _maxLinesY = (int)(scene.Camera.Size / _maxText.Size).Y - 1;
        _toDisplay = new List<BacklogRow>();

        _background = new Blank(Vector2.Zero, scene.Display.Size)
        {
            Color = new Color(0, 0, 0, 128)
        };

        _lines = new Text[_maxLinesY];
        for (int i = 0; i < _maxLinesY; i++)
        {
            _lines[i] = new Text(string.Empty, scene.Display.Scale * 4);
            _lines[i].Move(new Vector2(0, _maxText.Size.Y) * i);
        }

        _currentInput.ChangeText(string.Empty);
        _cursorDisplay = new Text("_", scene.Display.Scale * 4);

        _drawCursorInvoker = new OverTimeInvoker(500F);
        _drawCursorInvoker.Trigger += UpdateCursor;

        _currentInput.Move(new Vector2(0, scene.Camera.Size.Y - _maxText.Size.Y));

        Context = console is null ? new ContextProvider() : console.Context;
        ChangeColor(console?.GetColor() ?? [new Color(75, 75, 75)]);
    }

    private void UpdateCursor()
    {
        _isDrawingCursor = !_isDrawingCursor;
        _cursorDisplay.ChangeText(_isDrawingCursor ? "_" : "");
    }

    public void Update(GameTime gameTime)
    {
        _drawCursorInvoker.Update(gameTime);
        _background.Update(gameTime);
        _toDisplay = Backlog.GetRangeFromPointer(_maxLinesY);

        for (int line = 0; line < _lines.Length; line++)
        {
            Text l = _lines[line];
            l.Move(new Vector2(0, _maxText.Size.Y) * line);
            if (_toDisplay.Count > line)
            {
                var text = _toDisplay[line].Text;
                int i = text.Length;
                do
                {
                    // Quick fix to cut of overlapping lines.
                    // There should be a better solution like a linebreak but that would invoke effort!
                    l.ChangeText(text.Substring(0, i--));
                } while (l.Rectangle.Width > _scene.Camera.Size.X);

                l.ChangeColor(_toDisplay[line].ColorSet.Color);
            }
            else
                l.ChangeText(string.Empty);
        }

        _cursorDisplay.Move(_currentInput.Position + new Vector2(_currentInput.Size.X, 0));
        _cursorDisplay.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _background.Draw(spriteBatch);

        foreach (Text text in _lines)
            text.Draw(spriteBatch);
        _currentInput.Draw(spriteBatch);
        if (_isDrawingCursor)
            _cursorDisplay.Draw(spriteBatch);
    }

    /// <summary>
    /// Simulates User input to run a specific command.
    /// </summary>
    /// <param name="command"></param>
    public void RunCommand(string command)
    {
        char[] chars = command.ToCharArray();
        foreach (char c in chars)
            TextInput(null, new TextInputEventArgs(c));
        TextInput(null, new TextInputEventArgs('\n', Keys.Enter));
    }

    public void TextInput(object sender, TextInputEventArgs e)
    {
        string c = e.Character.ToString();

        if (c == "\b")
        {
            if (_currentInput.Length > 0)
                _currentInput.ChangeText(_currentInput.ToString()[..^1]);
            return;
        }

        if (e.Key != Keys.Enter)
        {
            var oldText = _currentInput.ToString();
            // get the maximum string for comparisons
            _currentInput.ChangeText(_currentInput + (c ?? string.Empty) + "_");
            int newMaxWidth = _currentInput.Rectangle.Width;

            // reset string after acquiring the length!
            _currentInput.ChangeText(oldText);

            // only add the new character if the size allows for one
            if (newMaxWidth < _scene.Camera.Size.X)
                _currentInput.ChangeText(_currentInput + c ?? string.Empty);

            return;
        }

        Backlog.Add(new BacklogRow(_currentInput.ToString()));
        var output = Processor.Process(this, _currentInput.ToString(), Context).Select(s => new BacklogRow(s));
        Backlog.AddRange(output);
        var length = output.Count();

        if (Backlog.Count > _maxLinesY)
            for (int i = -1; i < length; i++)
                Backlog.MovePointerDown();

        _currentInput.ChangeText(string.Empty);
    }

    public void Write(string text, int line = -1)
    {
        if (line == -1 || Backlog.Count <= line)
        {
            Backlog.Add(new BacklogRow(text));
            if (Backlog.Count > _maxLinesY)
                Backlog.MovePointerDown();
        }
        else
            Backlog[line].SetText(text);
    }

    public void WriteColor(string text, BacklogColorSet color, int line = -1)
    {
        if (line == -1 || Backlog.Count <= line)
        {
            Backlog.Add(new BacklogRow(text, color));
            if (Backlog.Count > _maxLinesY)
                Backlog.MovePointerDown();
        }
        else
        {
            Backlog[line].SetText(text);
            Backlog[line].SetColor(color);
        }
    }

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition;

        foreach (var line in _lines)
            line.Move(line.Position + offset);

        _currentInput.Move(_currentInput.Position + offset);
    }

    public void ChangeColor(Color[] input)
        => _color = input[0];

    public int ColorLength()
        => 1;

    public Color[] GetColor()
        => [_color];

    public void ScrollUp()
    {
        Backlog.MovePointerUp();
    }

    public void ScrollDown()
    {
        Backlog.MovePointerDown();
    }
}