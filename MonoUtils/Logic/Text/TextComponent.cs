using Newtonsoft.Json.Linq;

namespace MonoUtils.Logic.Text;

public sealed class TextComponent
{
    private readonly JArray _text;

    public TextComponent(string text)
    {
        _text = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(text)!;
    }

    public string GetValue(string key)
    {
        foreach (var item in _text)
        {
            var itemObject = item as JObject;
            if (itemObject == null)
                continue;

            if (!itemObject.TryGetValue("key", out var keyValue) || keyValue.ToString() != key)
                continue;

            if (!itemObject.TryGetValue("value", out var value))
                continue;

            return value.ToString();
        }

        return string.Empty;
    }
}