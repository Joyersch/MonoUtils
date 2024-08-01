using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.TextSystem;

namespace MonoUtils.Ui.Menu;

public sealed class LoadingScreen : IManageable
{
    private long _max;
    private string _goal;
    private long _current;

    private int _dots;

    public Rectangle Rectangle => _area;
    private Rectangle _area;

    private readonly Text _loading;
    private readonly Text _progress;
    private readonly Text _bar;
    private int _loadbarLength = 196;

    private OverTimeInvoker _lazyDots;
    private Rainbow _rainbowColor;

    public bool ProgressEnabled = true;


    public LoadingScreen(Vector2 position, Vector2 size, long max, float scale, string goal)
    {
        _max = max;
        _goal = goal;
        _area = new Rectangle(position.ToPoint(), size.ToPoint());

        _loading = new Text("Loading", 5F * scale);
        _loading.InRectangle(_area)
            .OnCenter()
            .OnY(0.25F)
            .Centered()
            .Move();

        _progress = new Text(goal, 2F * scale);
        _progress.InRectangle(_area)
            .OnCenter()
            .OnY(0.45F)
            .Centered()
            .Move();

        StringBuilder builder = new();
        for (int i = 0; i < _loadbarLength; i++)
            builder.Append('|');

        _bar = new Text(builder.ToString(), Vector2.Zero, 2F * scale, 0);
        _bar.InRectangle(_area)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Move();

        _lazyDots = new OverTimeInvoker(666F);
        _lazyDots.Trigger += () =>
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Loading");

            for (int i = 0; i < _dots; i++)
                builder.Append('.');

            _loading.ChangeText(builder.ToString());
            _dots++;
            if (_dots > 3)
                _dots = 0;
        };

        _rainbowColor = new Rainbow() { GameTimeStepInterval = 6F, Increment = 2, Offset = 64 };
    }

    public void SetCurrent(long current)
    {
        _current = current;
    }

    public void SetMax(long max)
    {
        _max = max;
    }

    public void SetGoal(string goal)
    {
        _goal = goal;
    }

    public void Update(GameTime gameTime)
    {

        _lazyDots.Update(gameTime);

        _loading.Update(gameTime);
        _loading.InRectangle(_area)
            .OnCenter()
            .OnY(0.25F)
            .Centered()
            .Move();

        if (ProgressEnabled)
        {
            _progress.ChangeText($"{_goal}: {_current}/{_max}");
            _progress.Update(gameTime);
            _progress.InRectangle(_area)
                .OnCenter()
                .OnY(0.45F)
                .Centered()
                .Move();
        }

        if (_max == 0)
            return;

        // if this crashes do to int long cast than something else is broken because this should be a percentage (0..1 * 30)
        int done = (int)(_loadbarLength * _current / _max);
        ColorBuilder colorBuilder = new ColorBuilder();

        _rainbowColor.Update(gameTime);

        colorBuilder.AddColor(_rainbowColor.GetColor(done));
        colorBuilder.AddColor(Microsoft.Xna.Framework.Color.White, _loadbarLength - done);

        _bar.ChangeColor(colorBuilder.GetColor());
        _bar.Update(gameTime);
        _bar.InRectangle(_area)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Move();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _bar.Draw(spriteBatch);
        if (ProgressEnabled)
            _progress.Draw(spriteBatch);
        _loading.Draw(spriteBatch);
    }
}