using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using IUpdateable = MonoUtils.Logic.IUpdateable;

namespace MonoUtils.Sound;

public sealed class LoopStation : IUpdateable, IDisposable
{
    private Dictionary<string, SoundEffect> _effects = new();
    private Dictionary<string, SoundEffectInstance> _instances = new();
    private Dictionary<string, float> _volumes = new();
    private float _masterVolume = 1F;
    private float _fadeTime;

    public void Register(SoundEffect effect, string name)
    {
        _effects.Add(name, effect);
    }

    public void Initialize(float fadeTime = 1F)
    {
        _fadeTime = fadeTime;
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
            var estimate = _volumes[instance.Key] * _masterVolume;
            if (estimate == instance.Value.Volume)
                continue;

            var offset = estimate - instance.Value.Volume;
            var apply = offset * (gameTime.ElapsedGameTime.TotalMilliseconds / _fadeTime);
            if (apply > estimate)
                apply = estimate;
            if (apply + instance.Value.Volume < 0D)
                apply = 0D;

            instance.Value.Volume += (float)apply;
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
        foreach (var key in _volumes.Keys)
        {
            _volumes[key] = 0;
        }
    }
}