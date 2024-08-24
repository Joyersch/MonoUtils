using Microsoft.Xna.Framework;
namespace MonoUtils.Logic.Hitboxes;

public interface IInteractable
{
    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck);
}