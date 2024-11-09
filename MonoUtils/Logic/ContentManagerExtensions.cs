using Audio = Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUtils.Logic;

public static class ContentManagerExtensions
{
    public static Texture2D Get(
        this Microsoft.Xna.Framework.Content.ContentManager contentManager, string textureName)
        => contentManager.Load<Texture2D>(textureName);

    public static Texture2D GetTexture(
        this Microsoft.Xna.Framework.Content.ContentManager contentManager, string textureName)
        => contentManager.Get("Textures/" + textureName);

    public static Audio.SoundEffect GetMusic(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<Audio.SoundEffect>("Music/" + name);

    public static Audio.SoundEffect GetSfx(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<Audio.SoundEffect>("SFX/" + name);

    public static Effect GetEffect(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<Effect>("Shaders/" + name);
}