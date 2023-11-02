using MonoUtils.Logic.Text;

namespace MonoUtils.Ui.Objects.Console.Commands;

public class LanguageCommand : ICommand
{
    [CommandAttribute(Description = "Change the used language", Name = "lang")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length < 1)
            return new[] { "Usage:", "lang [lang]", "Current:", TextProvider.Localization.ToString()};
        TextProvider.Language? toUse = null;
        if (options[0].ToString().ToLower().Contains("de"))
            toUse = TextProvider.Language.de_DE;
        if (options[0].ToString().ToLower().Contains("en"))
            toUse = TextProvider.Language.en_US;

        if (toUse is null)
            return new[] { "Unknown language" };
        TextProvider.Localization = toUse.Value;
        return new[] { "Changed language" };
    }
}