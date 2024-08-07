using System.Reflection;

namespace MonoUtils.Console;

public sealed class CommandProcessor : IProcessor
{
    public List<(string Name, string Description, ICommand Command)> Commands = new();

    public void Initialize()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        string @return = null;
        IEnumerable<Type> commands = null;
        foreach (var assembly in assemblies)
        {
            var classes = assembly.GetTypes().Where(t =>
                t.IsClass && t.GetMethods()
                    .Any(m => m.GetCustomAttribute<CommandAttribute>() is not null));
            commands = commands is null ? classes : commands.Concat(classes);
        }

        foreach (var command in commands)
        {
            var commandInstance = (ICommand)Activator.CreateInstance(command)!;
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

    public IEnumerable<string> Process(DevConsole caller, string fullCommand, ContextProvider context)
    {
        var commandSplit = fullCommand.Split(" ");

        if (Commands.All(c => c.Name != commandSplit[0]))
            return new[] { "This command does not exist!" };

        var command = Commands.FirstOrDefault(c => c.Name == commandSplit[0]);

        var options = new object[commandSplit.Length - 1];
        for (int i = 0; i < options.Length; i++)
            options[i] = commandSplit[i + 1];

        return command.Command.Execute(caller, options, context);
    }
}