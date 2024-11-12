namespace MonoUtils.Console.Commands;

public sealed class HelpCommand : ICommand
{
    [Command(Description = "Shows all command and there description.", Name = "help")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length == 0)
            return console.Processor.Commands.Select(command =>
                $"{command.Attribute.Name} - {command.Attribute.Description}");

        List<string> @return = [];

        if (console.Processor.Commands.All(command => command.Attribute.Name != options[0].ToString()))
            return [$"The given option \"{options[0]}\" does not match any existing commands"];

        var result =
            console.Processor.Commands.FirstOrDefault(command => command.Attribute.Name == options[0].ToString());

        @return.Add(result.Attribute.Description);
        if (result.Options.Length > 0)
            @return.Add("The following options are accepted:");
        foreach (var option in result.Options)
        {
            string message = option.Name;
            if ((option.Description ?? string.Empty).Trim().Length > 0)
                message += $" - {option.Description}";
            @return.Add(message);
        }

        return @return;
    }
}