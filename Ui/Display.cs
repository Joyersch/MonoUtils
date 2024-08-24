using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoUtils.Ui;

public sealed class Display
{
    public readonly RenderTarget2D Target;

    public static readonly float Width = 1280F;
    public static readonly float Height = 720F;

    // In theory Scale.X and Scale.Y should always be the same for this game
    public float SimpleScale => Scale.X;

    /// <summary>
    /// Area of the entire game window
    /// </summary>
    public Rectangle Window { get; private set; }

    /// <summary>
    /// Size of the entire game window
    /// </summary>
    public Vector2 WindowSize { get; private set; }

    /// <summary>
    /// Area of the <see cref="Target"/>.
    /// </summary>
    public Rectangle Screen { get; private set; }
    /// <summary>
    /// Size of the <see cref="Target"/>.
    /// </summary>
    public Vector2 Size { get; private set; }

    public Vector2 Scale => WindowSize / Size;

    //SHOUTOUT: https://youtu.be/yUSB_wAVtE8

    private readonly GraphicsDevice _device;

    public Display(GraphicsDevice device) : this(device, new Vector2(Width, Height))
    {
    }

    public Display(GraphicsDevice device, Vector2 size)
    {
        _device = device;
        Screen = _device.PresentationParameters.Bounds;
        WindowSize = _device.PresentationParameters.Bounds.Size.ToVector2();
        Target = new RenderTarget2D(device, (int)size.X, (int)size.Y);
    }

    public void Update()
    {
        Screen = _device.PresentationParameters.Bounds;
        WindowSize = _device.PresentationParameters.Bounds.Size.ToVector2();
        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        var backbufferAspectRatio = WindowSize.X / WindowSize.Y;
        var screenAspectRatio = (float)Target.Width / Target.Height;

        var x = 0f;
        var y = 0f;
        float w = Screen.Width;
        float h = Screen.Height;
        if (backbufferAspectRatio > screenAspectRatio)
        {
            w = h * screenAspectRatio;
            x = (Screen.Width - w) / 2f;
        }
        else if (backbufferAspectRatio < screenAspectRatio)
        {
            h = w / screenAspectRatio;
            y = (Screen.Height - h) / 2f;
        }

        Size = new Vector2(Target.Width, Target.Height);
        Window = new Rectangle((int)x, (int)y, (int)w, (int)h);
    }
}