using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Hitboxes;

namespace MonoUtils.Logic.Management;

public class ManagementCollection : List<IManageable>, IManageable, IInteractable
{
    public Rectangle Rectangle => Rectangle.Empty;

    public void Update(GameTime gameTime)
    {
        foreach (var manageable in this)
            manageable.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var manageable in this)
            manageable.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        foreach (var manageable in this)
        {
            if (manageable is IInteractable interactable)
                interactable.UpdateInteraction(gameTime, toCheck);
        }
    }
}