using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUtils.Logic;

public interface IDrawable : IRectangle
{
    public void Draw(SpriteBatch spriteBatch);
}