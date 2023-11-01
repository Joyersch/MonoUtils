using System.Reflection;
using MonoUtils.Logging;

namespace MonoUtils.Logic.Text;

public static class TextProvider
{
    private static List<string> Files = new();

    public static Language Localization { get; set; }

    public enum Language
    {
        de_DE,
        en_US
    }

    public static void Initialize()
    {
        var assembly = Assembly.GetCallingAssembly();
        var text = assembly.GetManifestResourceNames().Where(t => t.StartsWith("Text."));
        Files.AddRange(text);
    }

    public static TextComponent GetText(string name)
    {
        int index = Files.FindIndex(f => f == $"Text.{name}.{Localization}");
        if (index == -1)
            return new TextComponent(string.Empty);

        var assembly = Assembly.GetCallingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(Files[index]);
        using StreamReader reader = new StreamReader(stream);
        return new TextComponent(reader.ReadToEnd());
    }
}