using MonoUtils.Logging;

namespace MonoUtils.Settings;

public static class SettingsManager
{
    private static string _basePath;
    private static readonly List<ISettings> _settings;

    static SettingsManager()
    {
        _settings = new List<ISettings>();
        _settings.Add(new GeneralSettings());
    }

    public static void Add(ISettings settings)
        => _settings.Add(settings);

    public static T Get<T>()
    {
        return (T) _settings.First(s => s.Name == typeof(T).Name);
    }

    public static void Save()
    {
        foreach (var settings in _settings)
            settings.Save(_basePath);
    }

    public static void Load()
    {
        foreach (var settings in _settings)
        {
            try
            {
                settings.Load(_basePath);
            }
            catch (Exception exception)
            {
                Log.Write(exception.Message);
            }
        }
           
    }

    public static void SetBasePath(string path)
        => _basePath = path;
}