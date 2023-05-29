using Microsoft.Xna.Framework.Audio;

namespace MonoUtils.Sound;

public static class SoundEffectExtension
{
    public static SoundEffectInstance GetInstanceEx(this SoundEffect soundEffect, bool music)
        => Global.SoundSettingsListener.Register(soundEffect.CreateInstance(), music);
}