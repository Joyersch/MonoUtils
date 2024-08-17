using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Console;

public sealed class DevConsole : IManageable, ILayerable, IColorable, IMoveable
{
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _scale;
    private Color _color;

    private Text _currentInput;
    private Text _cursorDisplay;
    private Text _maxText;
    public Backlog Backlog { get; private set; }
    private List<BacklogRow> _toDisplay;
    private int _maxLinesY;
    private bool _isDrawingCursor;
    private OverTimeInvoker _drawCursorInvoker;

    public CommandProcessor Processor { get; private set; }

    public ContextProvider Context { get; private set; }

    private Text[] _lines;

    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;

    public float Layer { get; set; }

    public Vector2 ImageSize = new Vector2(128, 72);

    public static float DefaultScale { get; set; } = 5F;

    public static Texture2D Texture;

    public DevConsole(CommandProcessor processor, Vector2 position) : this(processor,
        position, DefaultScale, null)
    {
    }

    public DevConsole(CommandProcessor processor, Vector2 position, float scale) : this(processor, position, scale,
        null)
    {
    }

    public DevConsole(CommandProcessor processor, Vector2 position, float scale, DevConsole? console)
    {
        Processor = console is null ? processor : console.Processor;

        _size = ImageSize * scale * 5;
        _position = position;
        _scale = Vector2.One * scale * 5;
        _maxText = new Text("[block]", scale);
        _currentInput = new Text(string.Empty, scale);

        Backlog = console is null ? new Backlog() : console.Backlog;

        _maxLinesY = (int)(_size / _maxText.Size).Y - 1;
        _toDisplay = new List<BacklogRow>();

        _lines = new Text[_maxLinesY];
        for (int i = 0; i < _maxLinesY; i++)
        {
            _lines[i] = new Text(string.Empty, scale);
            _lines[i].Move(position + new Vector2(0, _maxText.Size.Y) * i);
        }

        _currentInput.ChangeText(string.Empty);
        _cursorDisplay = new Text("_", scale);

        _drawCursorInvoker = new OverTimeInvoker(500F);
        _drawCursorInvoker.Trigger += UpdateCursor;

        _currentInput.Move(position + new Vector2(0, _size.Y - _maxText.Size.Y));

        Context = console is null ? new ContextProvider() : console.Context;
        ChangeColor(console?.GetColor() ?? new[] { new Microsoft.Xna.Framework.Color(75, 75, 75) });
    }

    private void UpdateCursor()
    {
        _isDrawingCursor = !_isDrawingCursor;
        _cursorDisplay.ChangeText(_isDrawingCursor ? "_" : "");
    }

    public void Update(GameTime gameTime)
    {
        _rectangle = this.GetRectangle();
        _drawCursorInvoker.Update(gameTime);
        _toDisplay = Backlog.GetRangeFromPointer(_maxLinesY);

        for (int line = 0; line < _lines.Length; line++)
        {
            Text l = _lines[line];
            l.Move(GetPosition() + new Vector2(0, _maxText.Size.Y) * line);
            if (_toDisplay.Count > line)
            {
                var text = _toDisplay[line].Text;
                int i = text.Length;
                do
                {
                    // Quick fix to cut of overlapping lines.
                    // There should be a better solution like a linebreak but that would invoke effort!
                    l.ChangeText(text.Substring(0, i--));
                } while (l.Rectangle.Width > GetSize().X);

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
        spriteBatch.Draw(
            Texture,
            _position,
            null,
            _color,
            0F,
            Vector2.Zero,
            _scale,
            SpriteEffects.None,
            Layer);
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
        foreach(char c in chars)
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
            if (newMaxWidth < _size.X)
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
        var offset = newPosition - _position;

        foreach (var line in _lines)
            line.Move(line.Position + offset);

        _currentInput.Move(_currentInput.Position + offset);
        _position = newPosition;
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => _color = input[0];

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => new[] { _color };

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void ScrollUp()
    {
            Backlog.MovePointerUp();
    }

    public void ScrollDown()
    {
        Backlog.MovePointerDown();
    }
}