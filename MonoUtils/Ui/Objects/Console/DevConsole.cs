using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Console;

public class DevConsole : GameObject
{
    private readonly GameWindow _window;

    private Text _currentInput;
    public Backlog Backlog { get; private set; }
    private List<BacklogRow> _toDisplay;
    private int _maxLinesY;
    private bool _isDrawingCursor;
    private OverTimeInvoker _drawCursorInvoker;

    public CommandProcessor Processor { get; private set; }

    public ContextProvider Context { get; private set; }

    private Text[] _lines;

    public new static Vector2 DefaultSize = new Vector2(1280, 720) / 2;
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(128, 72),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 128, 72)
        }
    };

    public DevConsole(CommandProcessor processor, Vector2 position) : this(processor,
        position, 1F, null)
    {
    }

    public DevConsole(CommandProcessor processor, Vector2 position, float scale) : this(processor, position, scale,
        null)
    {
    }

    public DevConsole(CommandProcessor processor, Vector2 position, float scale, DevConsole? console) : base(position,
        DefaultSize * scale,
        DefaultTexture, DefaultMapping)
    {
        Processor = console is null ? processor : console.Processor;

        _currentInput = new Text(string.Empty, scale);


        Backlog = console is null ? new Backlog() : console.Backlog;

        _maxLinesY = (int)(Size / _currentInput.Size).Y - 1;
        _toDisplay = new List<BacklogRow>();

        _lines = new Text[_maxLinesY];
        for (int i = 0; i < _maxLinesY; i++)
        {
            _lines[i] = new Text(string.Empty, scale);
            _lines[i].Move(position + new Vector2(0, _currentInput.Size.Y) * i);
        }

        _drawCursorInvoker = new OverTimeInvoker(500F);
        _drawCursorInvoker.Trigger += UpdateCursor;

        _currentInput.Move(position + new Vector2(0, Size.Y - _currentInput.Size.Y));

        Context = console is null ? new ContextProvider() : console.Context;
        DrawColor = console?.DrawColor ?? new Microsoft.Xna.Framework.Color(75, 75, 75);
    }

    private void UpdateCursor()
    {
        _isDrawingCursor = !_isDrawingCursor;
        _currentInput.AppendText(_isDrawingCursor ? "_" : "\b");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _drawCursorInvoker.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.Up, true))
            Backlog.MovePointerUp();

        if (InputReaderKeyboard.CheckKey(Keys.Down, true))
            Backlog.MovePointerDown();

        _toDisplay = Backlog.GetRangeFromPointer(_maxLinesY);

        for (int line = 0; line < _lines.Length; line++)
        {
            Text l = _lines[line];
            l.Move(Position + new Vector2(0, _currentInput.Size.Y) * line);
            if (_toDisplay.Count > line)
            {
                var text = _toDisplay[line].Text;
                int i = text.Length;
                do
                {
                    // Quick fix to cut of overlapping lines.
                    // There should be a better solution like a linebreak but that would invoke effort!
                    l.ChangeText(text.Substring(0, i--));
                } while (l.Rectangle.Width > Size.X);

                l.ChangeColor(_toDisplay[line].ColorSet.Color);
            }
            else
                l.ChangeText(string.Empty);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        foreach (Text text in _lines)
            text.Draw(spriteBatch);
        _currentInput.Draw(spriteBatch);
    }

    public void TextInput(object sender, TextInputEventArgs e)
    {
        string c = e.Character.ToString();

        if (_isDrawingCursor)
            _currentInput.AppendText("\b");

        if (Letter.Parse(e.Character) == Letter.Character.Full
            && e.Character != '\b')
            c = string.Empty;


        if (e.Key != Keys.Enter)
        {
            var oldText = _currentInput.Value;
            // get the maximum string for comparisons
            _currentInput.AppendText((c ?? string.Empty) + "_");
            int newMaxWidth = _currentInput.Rectangle.Width;

            // reset string after acquiring the length!
            _currentInput.ChangeText(oldText);

            // only add the new character if the size allows for one
            if (newMaxWidth < Size.X)
                _currentInput.AppendText(c ?? string.Empty);

            if (_isDrawingCursor)
                _currentInput.AppendText("_");
            return;
        }

        var output = Processor.Process(this, _currentInput.Value, Context).Select(s => new BacklogRow(s));
        Backlog.Add(new BacklogRow(_currentInput.Value));
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

    public override void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;

        foreach (var line in _lines)
            line.Move(line.Position + offset);

        _currentInput.Move(_currentInput.Position + offset);
        Position = newPosition;
    }
}