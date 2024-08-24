using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUtils.Logic;

public interface IDrawable
{
    public Rectangle Rectangle { get; }

    public void Draw(SpriteBatch spriteBatch);
}