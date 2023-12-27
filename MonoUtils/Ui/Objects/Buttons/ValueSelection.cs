using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons;

public class ValueSelection : GameObject, IMoveable, IInteractable
{
    private readonly Text _display;
    private readonly SquareTextButton _decreaseButton;
    private readonly SquareTextButton _increaseButton;

    private readonly string _left = "[left]";
    private readonly string _right = "[right]";

    public event Action<object> ValueChanged;

    public List<object> ValidValues { get; private set; }

    public string Value => ValidValues[_pointer].ToString();

    public bool LoopOverValues = false;

    private int _pointer;

    public ValueSelection(Vector2 position, float scale, List<object> validValues, int startValueIndex) : base(
        position, SquareTextButton.DefaultSize * scale, DefaultTexture, DefaultMapping)
    {
        ValidValues = validValues;
        _pointer = startValueIndex;
        _decreaseButton = new SquareTextButton(position, scale, _left, _left);
        _decreaseButton.Click += DecreaseClicked;

        // get the longest value
        int longestValidValue = 0;
        foreach (var validValue in validValues)
        {
            var text = new Text(validValue.ToString());
            if (longestValidValue < text.Rectangle.Width)
                longestValidValue = text.Rectangle.Width;
        }

        _increaseButton =
            new SquareTextButton(position + new Vector2(_decreaseButton.Rectangle.Width + longestValidValue + 8, 0),
                scale, _right, _right);
        _increaseButton.Click += IncreaseClicked;

        Rectangle = Rectangle.Union(_decreaseButton.Rectangle, _increaseButton.Rectangle);
        Position = Rectangle.Location.ToVector2();
        Size = Rectangle.Size.ToVector2();

        _display = new Text(validValues[_pointer].ToString(),Vector2.Zero, scale);
        _display.GetCalculator(Rectangle)
            .OnCenter()
            .Centered()
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
        Move(Position);
        ValueChanged?.Invoke(ValidValues[_pointer]);
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;
        _decreaseButton.Move(_decreaseButton.Position + offset);

        _display.Move(_display.Position + offset);

        _increaseButton.Move(_increaseButton.Position + offset);
        Position = newPosition;
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _increaseButton.UpdateInteraction(gameTime, toCheck);
        _decreaseButton.UpdateInteraction(gameTime, toCheck);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _display.GetCalculator(Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        _display.Update(gameTime);
        _increaseButton.Update(gameTime);
        _decreaseButton.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
        _increaseButton.Draw(spriteBatch);
        _decreaseButton.Draw(spriteBatch);
    }

    protected override void UpdateRectangle()
    {
        Rectangle = Rectangle.Union(_decreaseButton.Rectangle, _increaseButton.Rectangle);
    }

}