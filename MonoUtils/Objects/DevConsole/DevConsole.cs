using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Objects.TextSystem;
using MonoUtils.Ui;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Objects;

public class DevConsole : GameObject, IMoveable
{
    private bool _isStatic;

    public new bool IsStatic
    {
        get => _isStatic;
        set
        {
            _isStatic = value;
            base.IsStatic = value;
            for (int i = 0; i < _maxLinesY; i++)
                _lines[i].IsStatic = value;
            _currentInput.IsStatic = value;
        }
    }

    private bool _isActive;
    public Keys Activator = Keys.F10;
    private Text _currentInput;
    private Backlog _backlog;
    private List<string> _toDisplay;
    private int _maxLinesY;
    private bool _isDrawingCursor;
    private OverTimeInvoker _drawCursorInvoker;

    public ContextProvider Context { get; private set; }

    private Text[] _lines;

    public new static Vector2 DefaultSize = new Vector2(1280, 720);
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(128, 72),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 128, 72)
        }
    };

    public DevConsole(GameWindow window, Vector2 position) : base(position, DefaultSize, DefaultTexture, DefaultMapping)
    {
    }

    public DevConsole(GameWindow window, Vector2 position, Vector2 size, float scale = 1F) : base(position, size,
        DefaultTexture, DefaultMapping)
    {
        window.TextInput += Window_TextInput;
        _currentInput = new Text(string.Empty, scale);

        _maxLinesY = (int) (size / _currentInput.Size).Y - 1;
        _backlog = new Backlog();
        _toDisplay = new List<string>();

        _lines = new Text[_maxLinesY];
        for (int i = 0; i < _maxLinesY; i++)
        {
            _lines[i] = new Text(string.Empty, scale);
            _lines[i].Move(position + new Vector2(0, _currentInput.Size.Y) * i);
        }

        _drawCursorInvoker = new OverTimeInvoker(500F);
        _drawCursorInvoker.Trigger += UpdateCursor;

        _currentInput.Move(new Vector2(0, size.Y - _currentInput.Size.Y));

        Context = new ContextProvider();
    }

    private void UpdateCursor()
    {
        _isDrawingCursor = !_isDrawingCursor;
        _currentInput.AppendText(_isDrawingCursor ? "_" : "\b");
    }

    public override void Update(GameTime gameTime)
    {
        if (InputReaderKeyboard.CheckKey(Activator, true))
            _isActive = !_isActive;

        if (!_isActive)
            return;

        base.Update(gameTime);

        _drawCursorInvoker.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.Up, true))
            _backlog.MovePointerUp();

        if (InputReaderKeyboard.CheckKey(Keys.Down, true))
            _backlog.MovePointerDown();

        _toDisplay = _backlog.GetRangeFromPointer(_maxLinesY);

        for (int line = 0; line < _lines.Length; line++)
        {
            _lines[line].Move(Position + new Vector2(0, _currentInput.Size.Y) * line);
            if (_toDisplay.Count > line)
                _lines[line].ChangeText(_toDisplay[line]);
            else
                _lines[line].ChangeText(string.Empty);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_isStatic || !_isActive)
            return;

        base.Draw(spriteBatch);
        foreach (Text text in _lines)
            text.Draw(spriteBatch);
        _currentInput.Draw(spriteBatch);
    }

    public override void DrawStatic(SpriteBatch spriteBatch)
    {
        if (!_isStatic || !_isActive)
            return;

        base.DrawStatic(spriteBatch);
        foreach (Text text in _lines)
            text.DrawStatic(spriteBatch);
        _currentInput.DrawStatic(spriteBatch);
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
            c = "";

        if (e.Key != Keys.Enter)
        {
            _currentInput.AppendText(c);
            if (_isDrawingCursor)
                _currentInput.AppendText("_");
            return;
        }

        var output = CommandProcessor.Process(this, _currentInput.Value, Context);
        _backlog.Add(_currentInput.Value);
        _backlog.AddRange(output);

        if (_backlog.Count > _maxLinesY)
            _backlog.MovePointerDown();

        _currentInput.ChangeText(string.Empty);
    }

    public void Write(string text, int line = -1)
    {
        if (line == -1 || _backlog.Count <= line)
            _backlog.Add(text);
        else
            _backlog[line] = text;
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;

        foreach (var line in _lines)
            line.Move(line.Position + offset);

        _currentInput.Move(_currentInput.Position + offset);
        Position = newPosition;
    }
}