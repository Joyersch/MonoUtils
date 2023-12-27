using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoUtils.Logging;
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
        Letter.Initialize();
        DefaultLetters.DefaultTexture = content.GetTexture("Font/DefaultLetters");
        ActionSymbols.DefaultTexture = content.GetTexture("Font/ActionSymbols");
        ButtonAddonIcons.DefaultTexture = content.GetTexture("Font/ButtonAddons");
        EmptyButton.DefaultTexture = content.GetTexture("emptybutton");
        MiniTextButton.DefaultTexture = content.GetTexture("minibutton");
        SquareTextButton.DefaultTexture = content.GetTexture("squarebutton");
        DevConsole.DefaultTexture = content.GetTexture("console");
        ConnectedGameObject.DefaultTexture = content.GetTexture("connectedsample");
    }

    public static string ReadFromResources(string file)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        string @return = null;
        foreach (var assembly in assemblies)
        {
            try
            {
                var files = assembly.GetManifestResourceNames();
                if (!files.Contains(file))
                    continue;
                using Stream stream = assembly.GetManifestResourceStream(file);
                using StreamReader reader = new StreamReader(stream);
                @return = reader.ReadToEnd();
            }
            catch(Exception exception)
            {
                Log.WriteError(exception.Message);
            }

            if (@return is not null)
                break;
        }

        return @return ?? string.Empty;
    }

    private static Color? _color;

    public static Color Color => _color ??= new Color(161, 0, 255); // #a100ff
}