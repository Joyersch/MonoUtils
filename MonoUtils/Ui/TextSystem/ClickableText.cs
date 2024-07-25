using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Ui.TextSystem;

public class ClickableText : Text, IInteractable, IHitbox
{
    private readonly float _scale;
    public event Action<object>? Leave;
    public event Action<object>? Enter;
    public event Action<object>? Click;

    private readonly MouseActionsMat _mouseActions;
    private readonly Text _highlight;

    public Rectangle[] Hitbox => new[] { Rectangle.Union(Rectangle, _highlight.Rectangle) };

    public static Microsoft.Xna.Framework.Color LinkColor = new(114, 158, 252);

    private bool _drawUnderscore;

    public ClickableText(string text) : this(text, 1F)
    {
    }

    public ClickableText(string text, float scale) : this(text, Vector2.Zero, scale)
    {
    }

    public ClickableText(string text, Vector2 position) : this(text, position, 1F)
    {
    }

    public ClickableText(string text, Vector2 position, float scale) : this(text, position, scale, 1)
    {
    }

    public ClickableText(string text, Vector2 position, float scale, int spacing) : base(text, position, scale, spacing)
    {
        _scale = scale;
        _mouseActions = new MouseActionsMat(this);
        _mouseActions.Enter += delegate
        {
            _drawUnderscore = true;
            Enter?.Invoke(this);
        };
        _mouseActions.Leave += delegate
        {
            _drawUnderscore = false;
            Leave?.Invoke(this);
        };
        _mouseActions.Click += o => Click?.Invoke(o);
        _highlight = new Text(string.Empty, position, scale, 0);
        _highlight.ChangeText(string.Concat(Enumerable.Repeat(".", (int)(Rectangle.Width / _scale))));
        _highlight.Move(position + new Vector2(0, _highlight.GetSize().Y));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _highlight.ChangeText(string.Concat(Enumerable.Repeat(".", (int)(Rectangle.Width / _scale))));
        _highlight.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseActions.UpdateInteraction(gameTime, toCheck);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (_drawUnderscore)
            _highlight.Draw(spriteBatch);
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        _highlight.Move(GetPosition() + new Vector2(0, _highlight.GetSize().Y));
    }

    public override void ChangeColor(Microsoft.Xna.Framework.Color[] color)
    {
        base.ChangeColor(color);
        _highlight.ChangeColor(color[0]);
    }

    public override void ChangeColor(Microsoft.Xna.Framework.Color color)
    {
        base.ChangeColor(color);
        _highlight.ChangeColor(color);
    }
}