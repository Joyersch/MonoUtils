using System.Reflection;
using MonoUtils.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoUtils.Settings;

public class SettingsManager
{
    private string _basePath;
    private int? _saveNumber;
    private readonly Dictionary<string, object> _settings;
    private readonly Dictionary<string, object> _saves;
    private readonly List<Type> _settingsImplementations;
    private readonly List<Type> _savesImplementations;

    public SettingsManager(string basePath, int? saveNumber = null)
    {
        _basePath = basePath;
        _saveNumber = saveNumber;
        _settings = new Dictionary<string, object>();
        _saves = new Dictionary<string, object>();
        _settingsImplementations = new List<Type>();
        _savesImplementations = new List<Type>();

        var settingsType = typeof(ISettings);
        var saveType = typeof(ISave);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var settingsImplementations = assembly.GetTypes()
                .Where(type =>
                    settingsType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            foreach (var implementation in settingsImplementations)
            {
                var instance = Activator.CreateInstance(implementation);
                if (instance is not ISettings)
                    continue;

                var type = instance.GetType();
                _settingsImplementations.Add(type);
                _settings.Add(type.Namespace.Split('.')[^1] + "." + type.Name, instance);
            }

            var saveImplementations = assembly.GetTypes()
                .Where(type =>
                    saveType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            foreach (var implementation in saveImplementations)
            {
                var instance = Activator.CreateInstance(implementation);
                if (instance is not ISave)
                    continue;

                var type = instance.GetType();
                _savesImplementations.Add(type);
                _saves.Add(type.Namespace.Split('.')[^1] + "." + type.Name, instance);
            }
        }
    }

    public void SetSaveFile(int save)
        => _saveNumber = save;

    public T GetSetting<T>() where T : ISettings
        => (T)_settings[typeof(T).Namespace.Split('.')[^1] + "." + typeof(T).Name];

    public T GetSave<T>() where T : ISave
        => (T)_saves[typeof(T).Namespace.Split('.')[^1] + "." + typeof(T).Name];

    public void Save()
    {
        SaveSave();
        SaveSettings();
    }

    public void SaveSave()
    {
        if (_saveNumber is null)
            return;
        string filePath = $@"{_basePath}/save_{_saveNumber}.json";
        SaveFile(filePath, _saves);
    }

    public void SaveSettings()
    {
        string filePath = $@"{_basePath}/settings.json";
        SaveFile(filePath, _settings);
    }

    private static void SaveFile(string filePath, Dictionary<string, object> collection)
    {
        FileStream stream = null;
        if (!File.Exists(filePath))
            stream = File.Create(filePath);

        string file = Newtonsoft.Json.JsonConvert.SerializeObject(collection);
        using StreamWriter writer = stream is null ? new StreamWriter(filePath) : new StreamWriter(stream);
        writer.Write(file);
    }

    public bool Load()
    {
        bool successSetting, successSave;
        successSetting = LoadSettings();
        successSave = LoadSaves();
        return successSetting && successSave;
    }

    public bool LoadSaves()
    {
        if (_saveNumber is null)
            return false;
        string filePath = $@"{_basePath}/save_{_saveNumber}.json";

        return LoadFile(filePath, _saves, _savesImplementations);
    }

    public bool LoadSettings()
    {
        string filePath = $@"{_basePath}/settings.json";

        return LoadFile(filePath, _settings, _settingsImplementations);
    }

    private static bool LoadFile(string filePath, Dictionary<string, object> collection, List<Type> implementations)
    {
        if (!File.Exists(filePath))
            return false;

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        foreach (var pair in jsonObject)
        {
            try
            {
                string key = pair.Key;
                JToken value = pair.Value;

                Type settingsType = implementations.First(i => i.Namespace.Split('.')[^1] + "." + i.Name == key);
                if (settingsType != null && collection.ContainsKey(key))
                {
                    object instance = JsonConvert.DeserializeObject(value.ToString(), settingsType);
                    collection[key] = instance;
                }
            }
            catch (Exception exception)
            {
                Log.WriteError($"Error while loading file with pair: {pair.Key} into: {nameof(collection)}");
                Log.WriteError(exception.Message);
            }
        }

        return true;
    }
}