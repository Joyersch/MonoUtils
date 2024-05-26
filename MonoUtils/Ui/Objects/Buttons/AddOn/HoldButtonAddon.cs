using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Objects.Buttons.AddOn;

public class HoldButtonAddon : ButtonAddon
{
    private readonly TextSystem.Text _timer;
    private readonly float _startTime;
    private float _time;

    private bool _finished;
    private bool _countDown;

    public HoldButtonAddon(IButton button, float startTime, float scale = 1F) : base(button)
    {
        _startTime = startTime;
        _time = _startTime;
        _timer = new Text($"{_startTime / 1000F:n2}", button.GetPosition(), scale);

        button.Click += delegate
        {
            _countDown = true;

            if (_finished)
                InvokeClick();
        };

        button.Leave += delegate
        {
            _countDown = false;
            InvokeLeave();
        };

        button.Enter += o => InvokeEnter();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _timer.Update(gameTime);

        if (_finished)
            return;

        _countDown = _countDown && Mouse.GetState().LeftButton == ButtonState.Pressed;

        if (_countDown)
            _time -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (!_finished && _time <= 0F)
        {
            _finished = true;
            _time = 0F;
            InvokeClick();
        }

        var newText = (_time / 1000).ToString("n2");
        _timer.ChangeText(newText);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _timer.Draw(spriteBatch);
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        _timer.Move(newPosition);
    }
}