using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public class HoldButtonAddon : ButtonAddonBase
{
    private readonly TextSystem.Text _timer;
    private readonly float _startTime;
    private bool _isHover;
    private float _time;
    private bool _hasReachedZero;
    private bool _pressStartOnObject;

    public HoldButtonAddon(ButtonAddonAdapter button, float startTime, float scale = 1F) : base(button, scale)
    {
        _startTime = startTime;
        _time = _startTime;
        _timer = new TextSystem.Text($"{_startTime / 1000F:n2}",
            Position, Scale);
        Size = _timer.Rectangle.Size.ToVector2();
        Button.SetIndicatorOffset((int) Size.X);
        _pressStartOnObject = !InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false);
    }



    protected override void ButtonCallback(object sender, IButtonAddon.CallState state)
    {
        if (state == IButtonAddon.CallState.Enter)
            _isHover = true;
        if (state == IButtonAddon.CallState.Leave)
            _isHover = false;
        if (_time == 0)
            base.ButtonCallback(sender, state);
    }

    public override void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        Button.UpdateInteraction(gameTime, toCheck);


        float passedGameTime = 0F;
        if (!_hasReachedZero)
            passedGameTime = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        if (_isHover && InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false))
        {
            if (!_pressStartOnObject)
            {
                _pressStartOnObject = InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true);
                return;
            }

            _time -= passedGameTime;
            if (_time <= 0 && !_hasReachedZero)
            {
                _time = 0;
                _hasReachedZero = true;
                base.ButtonCallback(Button, IButtonAddon.CallState.Click);
            }
        }
        else
        {
            _time += passedGameTime / 2;
            if (_time > _startTime)
                _time = _startTime;
        }

        string newText = string.Empty;

        // If _time has no value after decimal point there is no need to print the value after the decimal point
        newText = (_time / 1000).ToString("n2");
        _timer.ChangeText(newText);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _timer.Update(gameTime);
        Button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Button.Draw(spriteBatch);
        _timer.Draw(spriteBatch);
    }

    public override Vector2 GetPosition()
        => Button.GetPosition();

    public override Vector2 GetSize()
        => Button.GetSize();

    public override void SetDrawColor(Microsoft.Xna.Framework.Color color)
        => Button.SetDrawColor(color);

    public override Rectangle GetRectangle()
        => Button.GetRectangle();

    public override void Move(Vector2 newPosition)
    {
        Button.Move(newPosition);
        _timer.Move(newPosition);
        Position = newPosition;
    }

    public override void MoveIndicatorBy(Vector2 newPosition)
    {
        _timer.Move(_timer.Position + newPosition);
        Position += newPosition;
    }
}