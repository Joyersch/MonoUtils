using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Console;

namespace MonoUtils;

public class SimpleGame : Game
{
    protected readonly GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch;

    protected Display Display;
    protected SettingsManager SettingsManager;

    protected DevConsole Console;
    protected bool IsConsoleActive;
    protected bool IsConsoleEnabled;

    protected string SaveDirectory = "saves";
    protected int? SaveFile = null;

    protected InputKey ConsoleKey = new(Keys.F10);

    public SimpleGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        // This will also call LoadContent()
        base.Initialize();

        Display = new Display(GraphicsDevice);
        Window.TextInput += OnTextInput;

        Global.CommandProcessor.Initialize();

        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        Console = new DevConsole(Global.CommandProcessor, Vector2.Zero, Display.SimpleScale,
            Console);
        Log.Out = new LogAdapter(Console);

        SettingsManager = new SettingsManager(SaveDirectory);
        SettingsManager.SetSaveFile(SaveFile);

        if (!SettingsManager.LoadSettings())
            SettingsManager.SaveSettings();

        if (SaveFile is not null && !SettingsManager.LoadSaves())
            SettingsManager.SaveSave();
        
        TextProvider.Initialize();
    }
    
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Display.Update();

        if (InputReader.CheckKey(ConsoleKey, true))
            IsConsoleActive = !IsConsoleActive;

        if (IsConsoleActive && IsConsoleEnabled)
            Console.Update(gameTime);

        // This will store the last key states
        InputReaderMouse.StoreButtonStates();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        // Initialize the Textures of objects from MonoUtils
        Global.Initialize(Content);
    }

    public void ApplyFullscreen(bool fullscreen)
    {
        if (Graphics.IsFullScreen != fullscreen)
            Graphics.ToggleFullScreen();
    }

    public void ApplyFixedStep(bool fixedStep)
    {
        IsFixedTimeStep = fixedStep;
    }

    public void ApplyConsole(bool isEnabled)
        => IsConsoleEnabled = isEnabled;

    private void OnTextInput(object sender, TextInputEventArgs e)
    {
        if (Console is null)
            return;

        if (!IsConsoleActive)
            return;

        Console.TextInput(sender, e);
    }
}