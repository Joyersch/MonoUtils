using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUtils.Logic.Management;

public interface IManageable
{
    public Rectangle Rectangle { get; }
    public void Update(GameTime gameTime);

    public void Draw(SpriteBatch spriteBatch);
}