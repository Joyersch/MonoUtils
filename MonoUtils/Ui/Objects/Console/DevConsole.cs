using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Console;

public class DevConsole : GameObject
{
    private readonly GameWindow _window;

    private bool _isActive;
    public Keys? Activator = Keys.F10;
    private Text _currentInput;
    public Backlog Backlog { get; private set; }
    private List<string> _toDisplay;
    private int _maxLinesY;
    private bool _isDrawingCursor;
    private OverTimeInvoker _drawCursorInvoker;
    private bool _inputRegistered;

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

    public DevConsole(CommandProcessor processor, GameWindow window, Vector2 position) : this(processor, window,
        position, 1F, null)
    {
    }

    public DevConsole(CommandProcessor processor, GameWindow window, Vector2 position, float scale) : this(processor,
        window, position, scale, null)
    {
    }

    public DevConsole(CommandProcessor processor, GameWindow window, Vector2 position, float scale,
        DevConsole? console) : base(position,
        DefaultSize * scale,
        DefaultTexture, DefaultMapping)
    {
        _window = window;
        ActivateInput();

        Processor = console is null ? processor : console.Processor;

        _currentInput = new Text(string.Empty, scale);

        Backlog = console is null ? new Backlog() : console.Backlog;

        _maxLinesY = (int) (Size / _currentInput.Size).Y - 1;
        _toDisplay = new List<string>();

        _lines = new Text[_maxLinesY];
        for (int i = 0; i < _maxLinesY; i++)
        {
            _lines[i] = new Text(string.Empty, scale);
            _lines[i].Move(position + new Vector2(0, _currentInput.Size.Y) * i);
        }

        _drawCursorInvoker = new OverTimeInvoker(500F);
        _drawCursorInvoker.Trigger += UpdateCursor;

        _currentInput.Move(new Vector2(0, Size.Y - _currentInput.Size.Y));

        Context = console is null ? new ContextProvider() : console.Context;
        DrawColor = console?.DrawColor ?? DrawColor;
        _isActive = console?._isActive ?? false;
    }

    private void UpdateCursor()
    {
        _isDrawingCursor = !_isDrawingCursor;
        _currentInput.AppendText(_isDrawingCursor ? "_" : "\b");
    }

    public override void Update(GameTime gameTime)
    {
        if (Activator is not null && InputReaderKeyboard.CheckKey(Activator.Value, true))
            _isActive = !_isActive;

        if (!_isActive)
            return;

        base.Update(gameTime);

        if (_inputRegistered)
            _drawCursorInvoker.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.Up, true))
            Backlog.MovePointerUp();

        if (InputReaderKeyboard.CheckKey(Keys.Down, true))
            Backlog.MovePointerDown();

        _toDisplay = Backlog.GetRangeFromPointer(_maxLinesY);

        for (int line = 0; line < _lines.Length; line++)
        {
            _lines[line].Move(Position + new Vector2(0, _currentInput.Size.Y) * line);
            if (_toDisplay.Count > line)
                _lines[line].ChangeText(_toDisplay[line] ?? string.Empty);
            else
                _lines[line].ChangeText(string.Empty);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!_isActive)
            return;

        base.Draw(spriteBatch);
        foreach (Text text in _lines)
            text.Draw(spriteBatch);
        _currentInput.Draw(spriteBatch);
    }

    public void ActivateInput()
    {
        if (_inputRegistered)
            return;

        _window.TextInput += Window_TextInput;
        _inputRegistered = true;
    }

    public void DeactivateInput()
    {
        if (!_inputRegistered)
            return;

        _window.TextInput -= Window_TextInput;
        _inputRegistered = false;
    }

    private void Window_TextInput(object sender, TextInputEventArgs e)
    {
        string c = e.Character.ToString();
        if (e.Key == Activator)
            _isActive = !_isActive;

        if (!_isActive)
            return;

        if (_isDrawingCursor)
            _currentInput.AppendText("\b");

        if (Letter.Parse(e.Character) == Letter.Character.Full
            && e.Character != '\b')
            c = string.Empty;

        if (e.Key != Keys.Enter)
        {
            _currentInput.AppendText(c ?? string.Empty);
            if (_isDrawingCursor)
                _currentInput.AppendText("_");
            return;
        }

        var output = Processor.Process(this, _currentInput.Value, Context);
        Backlog.Add(_currentInput.Value);
        Backlog.AddRange(output);

        if (Backlog.Count > _maxLinesY)
            Backlog.MovePointerDown();

        _currentInput.ChangeText(string.Empty);
    }

    public void Write(string text, int line = -1)
    {
        if (line == -1 || Backlog.Count <= line)
            Backlog.Add(text);
        else
            Backlog[line] = text;
    }

    public override void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;

        foreach (var line in _lines)
            line.Move(line.Position + offset);

        _currentInput.Move(_currentInput.Position + offset);
        Position = newPosition;
    }

    public void Activate()
        => _isActive = true;

    public void Deactivate()
        => _isActive = false;
}