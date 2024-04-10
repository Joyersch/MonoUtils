using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Sound;

public class EffectsRegistry : IUpdateable
{
    private Dictionary<string, SoundEffect> _effects = new();
    private Dictionary<string, SoundEffectInstance> _instances = new();

    public void Register(SoundEffect effect, string key)
    {
        _effects.Add(key, effect);
    }

    public SoundEffectInstance? GetInstance(string key)
    {
        if (!_effects.ContainsKey(key))
            return null;
        var instance = _effects[key].CreateInstance();
        _instances.Add(key, instance);
        return instance;
    }

    public void Update(GameTime gameTime)
    {
        List<string> toNuke = new();
        foreach (var instance in _instances)
        {
            if (instance.Value.State == SoundState.Stopped)
                toNuke.Add(instance.Key);
        }

        foreach (var instance in toNuke)
        {
            var sound = _instances[instance];
            _instances.Remove(instance);
            sound.Dispose();
        }
    }
}