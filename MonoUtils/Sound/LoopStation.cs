using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Sound;

public class LoopStation : IUpdateable, IDisposable
{
    private Dictionary<string, SoundEffect> _effects = new();
    private Dictionary<string, SoundEffectInstance> _instances = new();
    private Dictionary<string, float> _volumes = new();
    private float _masterVolume = 1F;

    public void Register(SoundEffect effect, string name)
    {
        _effects.Add(name, effect);
    }

    public void Initialize()
    {
        foreach (var effect in _effects)
        {
            var instance = effect.Value.CreateInstance();
            instance.Volume = 0F;
            instance.IsLooped = true;
            instance.Play();
            _volumes.Add(effect.Key, 0F);
            _instances.Add(effect.Key, instance);
        }
    }

    public void SetMasterVolume(float volume)
        => _masterVolume = volume;

    public void Update(GameTime gameTime)
    {
        foreach (var instance in _instances)
        {
            instance.Value.Volume = _volumes[instance.Key] * _masterVolume;
        }
    }

    public void SetVolume(string key, float value)
        => _volumes[key] = value;

    public float GetVolume(string key)
        => _volumes[key];

    public void Dispose()
    {
        foreach (var instance in _instances)
        {
            if (!instance.Value.IsDisposed)
                instance.Value.Dispose();
        }
    }

    public void ResetVolume()
    {
        foreach (var instance in _instances)
        {
            instance.Value.Volume = 0F;
        }
    }
}