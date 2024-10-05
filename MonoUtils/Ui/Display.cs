using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;

namespace MonoUtils.Ui;

public sealed class Display : IRectangle, IScaleable
{
    private readonly GraphicsDevice _device;
    private readonly Vector2 _expectedSize;

    /// <summary>
    /// Rectangle of the current screen
    /// </summary>
    public Rectangle Window => _device.PresentationParameters.Bounds;

    public Rectangle Rectangle => Window;

    /// <summary>
    /// Size of the current screen
    /// </summary>
    public Vector2 Size => Window.Size.ToVector2();

    /// <summary>
    /// Scale between the current screen and the expected size
    /// </summary>
    public Vector2 ComplexScale => Size / _expectedSize;

    /// <summary>
    /// Simplified scale which takes the minimum of both of X and Y.
    /// </summary>
    public float Scale => Math.Min(ComplexScale.X, ComplexScale.Y);

    public event Action<float> OnResize;
    private Vector2 _lastSize;

    public Display(GraphicsDevice device, Vector2 expectedExpectedSize)
    {
        _device = device;
        _expectedSize = expectedExpectedSize;
    }

    public void Update()
    {
        if (_lastSize != Size)
            OnResize?.Invoke(Scale);

        _lastSize = Size;
    }

    /// <summary>
    /// This does nothing. Calls will be ignored. This exists for Interface complience
    /// </summary>
    /// <param name="scale"></param>
    public void SetScale(float scale)
    {
        // Ignored
    }
}