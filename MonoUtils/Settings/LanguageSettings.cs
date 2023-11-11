using MonoUtils.Logic.Text;

namespace MonoUtils.Settings;

public class LanguageSettings : ISettings
{
    public TextProvider.Language Localization { get; set; } = TextProvider.Language.en_US;
}