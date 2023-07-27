using System.Reflection;
using Microsoft.Xna.Framework.Content;
using MonoUtils.Logic;
using MonoUtils.Logic.Listener;
using MonoUtils.Sound;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Console;
using MonoUtils.Ui.Objects.TextSystem;

namespace MonoUtils;

public static class Global
{
    public static readonly SoundEffectsCache SoundEffects = new();

    public static readonly CommandProcessor CommandProcessor = new();

    public static SoundSettingsListener SoundSettingsListener;
    public static void Initialize(ContentManager content)
    {
        GameObject.DefaultTexture = content.GetTexture("placeholder");
        Cursor.DefaultTexture = content.GetTexture("cursor");
        Letter.DefaultTexture = content.GetTexture("font");
        EmptyButton.DefaultTexture = content.GetTexture("emptybutton");
        MiniTextButton.DefaultTexture = content.GetTexture("minibutton");
        SquareTextButton.DefaultTexture = content.GetTexture("squarebutton");
        DevConsole.DefaultTexture = content.GetTexture("console");
        ConnectedGameObject.DefaultTexture = content.GetTexture("connectedsample");
    }
    
    public static string ReadFromResources(string file)
    {
        var assembly = Assembly.GetCallingAssembly();
        if (!assembly.GetManifestResourceNames().Contains(file))
            throw new ArgumentException("Resource does not exists!");
        using Stream stream = assembly.GetManifestResourceStream(file);
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}