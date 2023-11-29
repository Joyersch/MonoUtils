using System.Reflection;
using MonoUtils.Logging;

namespace MonoUtils.Logic.Text;

public static class TextProvider
{
    private static List<string> Files = new();

    public static Language Localization { get; set; }

    public enum Language
    {
        en_GB,
        de_DE,
    }

    public static void Initialize()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            try
            {
                var text = assembly.GetManifestResourceNames().Where(t => t.StartsWith("Text."));
                Files.AddRange(text);
            }
            catch (Exception exception)
            {
                Log.WriteError(exception.Message);
            }
        }
    }

    public static TextComponent GetText(string name)
    {
        int index = Files.FindIndex(f => f == $"Text.{name}.{Localization}");
        if (index == -1)
            return new TextComponent(string.Empty);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            using Stream stream = assembly.GetManifestResourceStream(Files[index]);
            if (stream is null)
                continue;
            using StreamReader reader = new StreamReader(stream);
            return new TextComponent(reader.ReadToEnd());
        }

        return new TextComponent(string.Empty);
    }
}