using Microsoft.Xna.Framework.Audio;

namespace MonoUtils.Sound;

public class SoundEffectsCache
{
    private readonly Dictionary<string, (SoundEffect effect, bool isMusic)> _cache = new();

    public SoundEffectInstance GetInstance(string key, bool isMusic)
        => GetEffect(key).GetInstanceEx(isMusic);
    
    public SoundEffectInstance GetSfxInstance(string key)
        => GetEffect(key).GetInstanceEx(false);

    public SoundEffectInstance GetMusicInstance(string key)
        => GetEffect(key).GetInstanceEx(true);

    public SoundEffect GetEffect(string key)
        => _cache.FirstOrDefault(x => x.Key == key).Value.effect;

    public bool AddMappingToCache(string key, SoundEffect effect, bool isMusic)
        => _cache.TryAdd(key, (effect, isMusic));
    
    public bool AddSfxToCache(string key, SoundEffect effect)
        => _cache.TryAdd(key, (effect, false));
    
    public bool AddMusicToCache(string key, SoundEffect effect)
        => _cache.TryAdd(key, (effect, true));
}