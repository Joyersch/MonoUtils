namespace MonoUtils.Settings;

public static class FileManager
{
    public static object LoadFile(string path, Type type)
    {
        using var streamReader = new StreamReader(path);
        return Newtonsoft.Json.JsonConvert.DeserializeObject(streamReader.ReadToEnd(), type);
    }

    public static void SaveFile(string path, object data)
    {
        using var streamWriter = new StreamWriter(path);
        streamWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data));
    }
}