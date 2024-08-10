using Microsoft.Xna.Framework.Content;

namespace MonoUtils.Logic;

public interface IHasContent
{
    public void LoadContent(ContentManager content);
}