using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Ui.Objects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IInteractable
{
    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    private SoundEffectInstance _soundEffectInstance;
    private MouseActionsMat _mouseMat;

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize * 4;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(32, 16),
        Hitboxes = new[]
        {
            new Rectangle(2, 1, 28, 14),
            new Rectangle(1, 2, 30, 12)
        }
    };

    public EmptyButton() : this(Vector2.Zero, DefaultSize)
    {
    }

    public EmptyButton(Vector2 position) : this(position, DefaultSize)
    {
    }

    public EmptyButton(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public EmptyButton(Vector2 position, Vector2 size) : this(position, size, DefaultTexture, DefaultMapping)
    {
    }

    public EmptyButton(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping) :
        base(position, size, texture, mapping)
    {
        _mouseMat = new MouseActionsMat(this);
        _mouseMat.Leave += _ => InvokeLeaveEventHandler();
        _mouseMat.Enter += _ => InvokeEnterEventHandler();
        _mouseMat.Click += _ => InvokeClickEventHandler();
    }

    public virtual void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseMat.UpdateInteraction(gameTime, toCheck);

        ImageLocation = new Rectangle(_mouseMat.IsHover ? (int)TextureHitboxMapping.ImageSize.X : 0, 0,
            (int)TextureHitboxMapping.ImageSize.X, (int)TextureHitboxMapping.ImageSize.Y);
    }

    protected void InvokeClickEventHandler()
    {
        Click?.Invoke(this);
    }

    protected void InvokeEnterEventHandler()
        => Enter?.Invoke(this);

    protected void InvokeLeaveEventHandler()
        => Leave?.Invoke(this);


    public bool IsHover() => _mouseMat.IsHover;
}