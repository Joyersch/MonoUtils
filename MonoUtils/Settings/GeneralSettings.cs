using MonoUtils.Logic;

namespace NoNameButtonGame.Storage;

public class GeneralSettings : ISettings, IChangeable
{
    public string Name => nameof(GeneralSettings);

    public void Load(string path)
    {
        var copy = (GeneralSettings) FileManager.LoadFile($"{path}/{Name}.json", typeof(GeneralSettings));
        IsFullscreen = copy.IsFullscreen;
        IsFixedStep = copy.IsFixedStep;
        MusicVolume = copy.MusicVolume;
        SfxVolume = copy.SfxVolume;
        Resolution = copy.Resolution;
        Resolution.HasChanged += (_, _) => HasChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Save(string path)
        => FileManager.SaveFile($"{path}/{Name}.json", this);

    public object Get()
        => this;

    public Resolution Resolution { get; set; }
    public event EventHandler HasChanged;

    private bool _isFixedStep;

    public bool IsFixedStep
    {
        get => _isFixedStep;
        set
        {
            _isFixedStep = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool _isFullscreen;

    public bool IsFullscreen
    {
        get => _isFullscreen;
        set
        {
            _isFullscreen = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _musicVolume;

    public int MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _sfxVolume;

    public int SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public GeneralSettings()
    {

        Resolution = new Resolution();
        Resolution.Width = 1280;
        Resolution.Height = 720;
        IsFullscreen = false;
        IsFixedStep = true;
        MusicVolume = 5;
        SfxVolume = 5;
        Resolution.HasChanged += (_, _) => HasChanged?.Invoke(this, EventArgs.Empty);
    }
}