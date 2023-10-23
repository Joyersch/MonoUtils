using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils.Ui.Menu;

public class LoadingScreen : GameObject
{
    private int _max;
    private int _current;

    private int _dots;

    private Rectangle _area;

    private readonly Text _loading;
    private readonly Text _progress;
    private readonly Text _bar;

    private OverTimeInvoker _lazyDots;
    private Rainbow _rainbowColor;

    public bool ProgressEnabled = true;


    public LoadingScreen(Vector2 position, Vector2 size, int max, float scale) : base(position, size)
    {
        _max = max;
        _area = new Rectangle(position.ToPoint(), size.ToPoint());

        _loading = new Text("Loading", 5F * scale);
        _loading.GetCalculator(_area)
            .OnCenter()
            .OnY(0.25F)
            .Centered()
            .Move();

        _progress = new Text(string.Empty, 2F * scale);
        _progress.GetCalculator(_area)
            .OnCenter()
            .OnY(0.45F)
            .Centered()
            .Move();

        _bar = new Text("⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜", Vector2.Zero, 2F * scale, 0);
        _bar.GetCalculator(_area)
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

        _rainbowColor = new Rainbow() { GameTimeStepInterval = 50F, Increment = 5 };
    }

    public void SetCurrent(int current)
    {
        _current = current;
    }

    public void SetMax(int max)
    {
        _max = max;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _lazyDots.Update(gameTime);

        _loading.Update(gameTime);
        _loading.GetCalculator(_area)
            .OnCenter()
            .OnY(0.25F)
            .Centered()
            .Move();

        if (ProgressEnabled)
        {
            _progress.ChangeText($"Progress: {_current}/{_max}");
            _progress.Update(gameTime);
            _progress.GetCalculator(_area)
                .OnCenter()
                .OnY(0.45F)
                .Centered()
                .Move();
        }


        int done = 30 * _current / _max;
        ColorBuilder colorBuilder = new ColorBuilder();

        _rainbowColor.Update(gameTime);

        colorBuilder.AddColor(_rainbowColor.GetColor(done));
        colorBuilder.AddColor(Microsoft.Xna.Framework.Color.White, 30 - done);

        _bar.ChangeColor(colorBuilder.GetColor());
        _bar.Update(gameTime);
        _bar.GetCalculator(_area)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Move();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _bar.Draw(spriteBatch);
        if (ProgressEnabled)
            _progress.Draw(spriteBatch);
        _loading.Draw(spriteBatch);
    }
}