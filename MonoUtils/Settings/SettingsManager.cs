using System.Reflection;
using MonoUtils.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoUtils.Settings;

public class SettingsManager
{
    private string _basePath;
    private readonly int _saveNumber;
    private readonly Dictionary<string, object> _settings;
    private readonly List<Type> _settingsImplementations;

    public SettingsManager(string basePath, int saveNumber)
    {
        _basePath = basePath;
        _saveNumber = saveNumber;
        _settings = new Dictionary<string, object>();
        _settingsImplementations = new List<Type>();

        var settingsType = typeof(ISettings);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var settingsImplementations = assembly.GetTypes()
                .Where(type =>
                    settingsType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            foreach (var implementation in settingsImplementations)
            {
                var instance = Activator.CreateInstance(implementation);
                if (instance is not ISettings settingsInstance)
                    continue;

                var type = instance.GetType();
                _settingsImplementations.Add(type);
                _settings.Add(type.Namespace.Split('.')[^1] + "." +  type.Name, instance);
            }
        }
    }

    public T GetSetting<T>() where T : ISettings
        => (T)_settings[typeof(T).Namespace.Split('.')[^1] + "." + typeof(T).Name];

    public void Save()
    {
        string filePath = $@"{_basePath}/save_{_saveNumber}.json";
        FileStream stream = null;
        if (!File.Exists(filePath))
            stream = File.Create(filePath);

        string file = Newtonsoft.Json.JsonConvert.SerializeObject(_settings);
        using StreamWriter writer = stream is null ? new StreamWriter(filePath) : new StreamWriter(stream);
        writer.Write(file);
    }

    public bool Load()
    {
        string filePath = $@"{_basePath}/save_{_saveNumber}.json";

        if (!File.Exists(filePath))
            return false;

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        foreach (var pair in jsonObject)
        {
            string key = pair.Key;
            JToken value = pair.Value;

            Type settingsType = _settingsImplementations.First(i => i.Namespace.Split('.')[^1] + "." + i.Name == key);
            if (settingsType != null && _settings.ContainsKey(key))
            {
                object settingsInstance = JsonConvert.DeserializeObject(value.ToString(), settingsType);
                _settings[key] = settingsInstance;
            }
        }
        return true;
    }
}