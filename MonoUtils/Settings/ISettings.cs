namespace MonoUtils.Settings;

public interface ISettings
{
    public string Name { get; }
    public void Load(string path);
    public void Save(string path);

    public object Get();
}