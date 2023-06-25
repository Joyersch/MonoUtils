using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Objects.TextSystem;

namespace MonoUtils.Objects;

public class DevConsole : GameObject
{
    private bool _isStatic;

    public new bool IsStatic
    {
        get => _isStatic;
        set
        {
            _isStatic = value;
            for (int i = 0; i < _maxLinesY; i++)
                _lines[i].IsStatic = value;
            _currentInput.IsStatic = value;
            background.IsStatic = value;
        }
    }

    private Text _currentInput;
    private Backlog _backlog;
    private List<string> _toDisplay;
    private int _maxLinesY;
    private GameObject background;

    private Text[] _lines;

    public DevConsole(Vector2 position, Vector2 size, float scale = 1F) : base(position, size)
    {
        background = new GameObject(position, size);
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

        _currentInput.Move(new Vector2(0, size.Y - _currentInput.Size.Y));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _currentInput.AppendText(InputProcessorKeyboard.GetPressedText());

        if (InputReaderKeyboard.CheckKey(Keys.Up, true))
            _backlog.MovePointerUp();

        if (InputReaderKeyboard.CheckKey(Keys.Down, true))
            _backlog.MovePointerDown();

        if (InputReaderKeyboard.CheckKey(Keys.Enter, true))
        {
            List<string> output = CommandProcessor.Process(_currentInput.Value);
            _backlog.Add(_currentInput.Value);
            _backlog.AddRange(output);
            if (_backlog.Count > _maxLinesY)
                _backlog.MovePointerDown();
            _currentInput.ChangeText(string.Empty);
        }

        _toDisplay = _backlog.GetRangeFromPointer(_maxLinesY);
        for (int line = 0; line < _lines.Length; line++)
        {
            if (_toDisplay.Count > line)
                _lines[line].ChangeText(_toDisplay[line]);
            else
                _lines[line].ChangeText(string.Empty);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_isStatic)
            return;

        background.Draw(spriteBatch);
        foreach (Text text in _lines)
            text.Draw(spriteBatch);
        _currentInput.Draw(spriteBatch);
    }

    public override void DrawStatic(SpriteBatch spriteBatch)
    {
        if (!_isStatic)
            return;

        background.DrawStatic(spriteBatch);
        foreach (Text text in _lines)
            text.DrawStatic(spriteBatch);
        _currentInput.DrawStatic(spriteBatch);
    }
}