using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons;

public class ValueSelection<T> : IManageable, IMoveable, IInteractable
{
    private Vector2 _position;
    private readonly Vector2 _size;
    private readonly Vector2 _scale;
    private Microsoft.Xna.Framework.Color _color;
    private Rectangle _rectangle;

    private readonly Text _display;
    private readonly SquareTextButton _decreaseButton;
    private readonly SquareTextButton _increaseButton;

    private readonly string _left = "[left]";
    private readonly string _right = "[right]";

    public event Action<object> ValueChanged;

    public Rectangle Rectangle => _rectangle;

    public List<T> ValidValues { get; private set; }

    public string Value => ValidValues[_pointer].ToString();

    public bool LoopOverValues = false;

    private int _pointer;
    private int _longestValidValue;

    public ValueSelection(Vector2 position, float scale, List<T> validValues, int startValueIndex)
    {
        _position = position;
        ValidValues = validValues;
        _pointer = startValueIndex;
        _decreaseButton = new SquareTextButton(_left, position, scale * 4F);
        _decreaseButton.Click += DecreaseClicked;

        // get the longest value
        _longestValidValue = 0;
        foreach (var validValue in validValues)
        {
            var text = new Text(validValue.ToString());
            if (_longestValidValue < text.Rectangle.Width)
                _longestValidValue = text.Rectangle.Width;
        }

        var buttonLength = _decreaseButton.Rectangle.Width + 8;
        var decreaseAndTextSize = new Vector2(_longestValidValue + buttonLength, 0);
        var increasePosition = position + decreaseAndTextSize;
        _increaseButton = new SquareTextButton(_right, increasePosition, scale * 4F);
        _increaseButton.Click += IncreaseClicked;

        var rectangle = new Rectangle(position.ToPoint(), new Vector2(_longestValidValue + buttonLength * 2, _decreaseButton.GetSize().Y).ToPoint());
        _rectangle = rectangle;
        _size = rectangle.Size.ToVector2();

        _display = new Text(validValues[_pointer].ToString(), Vector2.Zero, scale);
        _display.GetCalculator(Rectangle)
            .OnCenter()
            .Centered()
            .Move();

        var halfText = _display.GetSize().X / 2;
        var longestHalfText = _longestValidValue / 2;
        var distance = longestHalfText - halfText;

        _decreaseButton.GetAnchor(_display)
            .SetMainAnchor(AnchorCalculator.Anchor.Left)
            .SetSubAnchor(AnchorCalculator.Anchor.Right)
            .SetDistanceX(distance + 8)
            .Move();

        _increaseButton.GetAnchor(_display)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(distance + 8)
            .Move();
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

        var halfText = _display.GetSize().X / 2;
        var longestHalfText = _longestValidValue / 2;
        var distance = longestHalfText - halfText;

        _display.GetCalculator(Rectangle)
            .OnCenter()
            .Centered()
            .Move();

        _decreaseButton.GetAnchor(_display)
            .SetMainAnchor(AnchorCalculator.Anchor.Left)
            .SetSubAnchor(AnchorCalculator.Anchor.Right)
            .SetDistanceX(distance + 8)
            .Move();

        _increaseButton.GetAnchor(_display)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(distance + 8)
            .Move();

        ValueChanged?.Invoke(ValidValues[_pointer]);
    }


    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        _decreaseButton.Move(_decreaseButton.GetPosition() + offset);

        _display.Move(_display.Position + offset);

        _increaseButton.Move(_increaseButton.GetPosition() + offset);
        _position = newPosition;
        _rectangle = this.GetRectangle();
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

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;
}