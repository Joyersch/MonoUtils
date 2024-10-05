using MonoUtils.Ui;

namespace MonoUtils.Logic;

public class DynamicScaler
{
    List<IScaleable> _scaleables;
    public DynamicScaler(Display display)
    {
        _scaleables = [];
        display.OnResize += Apply;
    }

    public void Apply(float scale)
    {
        foreach (var scaleable in _scaleables)
            scaleable.SetScale(scale);
    }

    public void Register(IScaleable scale)
        => _scaleables.Add(scale);
}