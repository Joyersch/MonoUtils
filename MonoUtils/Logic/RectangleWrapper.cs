using Microsoft.Xna.Framework;

namespace MonoUtils.Logic;

public class RectangleWrapper : IRectangle
{
    public Rectangle Rectangle { private set; get; }

    public RectangleWrapper(Rectangle rectangle)
    {
        Rectangle = rectangle;
    }
}