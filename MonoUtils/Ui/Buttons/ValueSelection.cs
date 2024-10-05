using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Ui.Buttons;

public class ValueSelection<T> : IManageable, IMoveable, IInteractable, IScaleable
{
    private readonly Text _display;
    private readonly SquareTextButton _decreaseButton;
    private readonly SquareTextButton _increaseButton;

    private Vector2 _position;
    private Vector2 _size;
    private Rectangle _rectangle;

    private float _initialScale;
    private float _extendedScale = 1F;
    public float Scale => _extendedScale * _extendedScale;
    public Rectangle Rectangle => _rectangle;

    private readonly string _left = "[left]";
    private readonly string _right = "[right]";

    public event Action<object> ValueChanged;

    public List<T> ValidValues { get; private set; }

    public string Value => ValidValues[_pointer].ToString();

    private int _pointer;
    private int _longestValidValue;

    public bool LoopOverValues;

    public ValueSelection(Vector2 position, float initialScale, List<T> validValues, int startValueIndex)
    {
        ValidValues = validValues;
        _position = position;
        _initialScale = initialScale;
        _pointer = startValueIndex;

        _decreaseButton = new SquareTextButton(_left, position, initialScale * SquareTextButton.DefaultScale);
        _decreaseButton.Click += DecreaseClicked;

        _increaseButton = new SquareTextButton(_right, Vector2.Zero, Scale * SquareTextButton.DefaultScale);
        _increaseButton.Click += IncreaseClicked;

        _display = new Text(validValues[_pointer].ToString(), Vector2.Zero, Scale * Text.DefaultLetterScale);
        UpdateTextValue();
    }

    private void IncreaseClicked(object obj)
    {
        _pointer++;
        if (_pointer > ValidValues.Count - 1)
            _pointer = LoopOverValues ? 0 : ValidValues.Count - 1;
        UpdateTextValue();
    }

    private void DecreaseClicked(object obj)
    {
        _pointer--;
        if (_pointer < 0)
            _pointer = LoopOverValues ? ValidValues.Count - 1 : 0;
        UpdateTextValue();
    }

    private void UpdateTextValue()
    {
        _display.ChangeText(Value);

        _longestValidValue = 0;
        foreach (var validValue in ValidValues)
        {
            var text = new Text(validValue.ToString());
            text.SetScale(_extendedScale);
            if (_longestValidValue < text.Rectangle.Width)
                _longestValidValue = text.Rectangle.Width;
        }

        var buttonLength = _decreaseButton.Rectangle.Width + 8;

        var rectangle = new Rectangle(_position.ToPoint(),
            new Vector2(_longestValidValue + buttonLength * 2, _decreaseButton.GetSize().Y).ToPoint());
        _rectangle = rectangle;
        _size = rectangle.Size.ToVector2();

        _display.InRectangle(this)
            .OnCenter()
            .Centered()
            .Apply();

        var halfText = _display.GetSize().X / 2;
        var longestHalfText = _longestValidValue / 2;
        var distance = (longestHalfText - halfText) / 2;

        _decreaseButton.GetAnchor(this)
            .SetMainAnchor(AnchorCalculator.Anchor.Left)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .Apply();

        _increaseButton.GetAnchor(this)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Right)
            .Apply();

        ValueChanged?.Invoke(ValidValues[_pointer]);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _increaseButton.UpdateInteraction(gameTime, toCheck);
        _decreaseButton.UpdateInteraction(gameTime, toCheck);
    }

    public void Update(GameTime gameTime)
    {
        _display.Update(gameTime);
        _increaseButton.Update(gameTime);
        _decreaseButton.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
        _increaseButton.Draw(spriteBatch);
        _decreaseButton.Draw(spriteBatch);
    }

    public void SetScale(float scale)
    {
        _display.SetScale(scale);
        _increaseButton.SetScale(scale);
        _decreaseButton.SetScale(scale);
        _extendedScale = scale;
        UpdateTextValue();
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
        UpdateTextValue();
    }
}