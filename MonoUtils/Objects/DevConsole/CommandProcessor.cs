using System.Reflection;
using Microsoft.Xna.Framework;
using MonoUtils.Logging;

namespace MonoUtils.Objects;

public static class CommandProcessor
{
    public static List<(string Name, string Description, ICommand Command)> Commands = new();

    public static void Initialize()
    {
        var commandsClasses = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
            t.IsClass && t.GetMethods().Any(m => m.GetCustomAttribute<CommandAttribute>() is not null));
        
        foreach (var command in commandsClasses)
        {
            var commandInstance = (ICommand)Activator.CreateInstance(command);
            var methods = command.GetMethods();
            
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<CommandAttribute>();
                if (attribute is null)
                    continue;

                Commands.Add((attribute.Name, attribute.Description, commandInstance));
            }
        }
    }

    public static IEnumerable<string> Process(DevConsole caller, string fullCommand)
    {
        var commandSplit = fullCommand.Split(" ");

        if (Commands.All(c => c.Name != commandSplit[0]))
            return new string[] { "This command does not exist!" };
        
        var command = Commands.FirstOrDefault(c => c.Name == commandSplit[0]);

        var options = new object[commandSplit.Length];
        for (int i = 0; i < commandSplit.Length; i++)
            options[i] = commandSplit[i];
        options[0] = caller;
        
        return command.Command.Execute(options);
    }
}