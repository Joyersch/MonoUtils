using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Ui.Objects;

public class Invisible : GameObject, IInteractable, IMouseActions
{
    private MouseActionsMat _mouseActionsMat;

    public event Action<object>? Leave;
    public event Action<object>? Enter;
    public event Action<object>? Click;

    public Invisible()
    {
    }

    public Invisible(Vector2 size) : this(Vector2.Zero, size)
    {
    }

    public Invisible(Vector2 position, Vector2 size) : base(position, size)
    {
        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Click += _ => Click?.Invoke(this);
        _mouseActionsMat.Enter += _ => Enter?.Invoke(this);
        _mouseActionsMat.Leave += _ => Leave?.Invoke(this);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseActionsMat.UpdateInteraction(gameTime, toCheck);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
    }
}