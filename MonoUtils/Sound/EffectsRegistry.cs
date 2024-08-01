using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Sound;

public sealed class EffectsRegistry : IUpdateable
{
    private Dictionary<string, SoundEffect> _effects = new();
    private List<SoundEffectInstance> _instances = new();
    private float _masterVolume;

    public void Register(SoundEffect effect, string key)
    {
        _effects.Add(key, effect);
    }

    public float GetMasterVolume()
        => _masterVolume;

    public void SetMasterVolume(float volume)
        => _masterVolume = volume;

    public SoundEffectInstance? GetInstance(string key)
    {
        if (!_effects.ContainsKey(key))
            return null;

        var instance = _effects[key].CreateInstance();
        instance.Volume = _masterVolume;
        _instances.Add(instance);
        return instance;
    }

    public void Update(GameTime gameTime)
    {
        for (var index = _instances.Count - 1; index >= 0; index--)
        {
            var instance = _instances[index];
            if (instance.State == SoundState.Stopped)
            {
                _instances.Remove(instance);
                instance.Dispose();
            }
        }
    }
}