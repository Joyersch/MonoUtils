using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Console;
using MonoUtils.Logging;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;

namespace MonoUtils;

public class ExtendedGame : Game
{
    protected readonly GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch;

    protected Scene Scene;
    protected SettingsAndSaveManager<string> SettingsAndSaveManager;

    protected DevConsole Console;
    protected bool IsConsoleActive;
    protected bool IsConsoleEnabled;

    protected Vector2 ScreenSize = new(1280F, 720F);

    protected string SaveDirectory = "saves";
    protected string SaveFile = string.Empty;

    protected float CameraZoom = 1F;

    protected bool Debug;

    protected string[] Args;

    protected bool ConsoleVisible => IsConsoleActive && IsConsoleEnabled;

    public ExtendedGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        // This will also call LoadContent()
        base.Initialize();

        Args = Environment.GetCommandLineArgs();

        if (Args.Contains("--debug"))
            Debug = true;

        Scene = new Scene(GraphicsDevice, ScreenSize, CameraZoom);
        Window.TextInput += OnTextInput;

        Global.CommandProcessor.Initialize();

        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        Console = new DevConsole(Global.CommandProcessor, Scene, Console);
        Log.Out = new LogAdapter(Console);

        SettingsAndSaveManager = new SettingsAndSaveManager<string>(SaveDirectory, SaveFile);
        SettingsAndSaveManager.SetSaveFile(SaveFile);

        if (!SettingsAndSaveManager.LoadSettings())
            SettingsAndSaveManager.SaveSettings();

        if (SaveFile is not null && !SettingsAndSaveManager.LoadSaves())
            SettingsAndSaveManager.SaveSave();
        
        TextProvider.Initialize();
    }

    /// <summary>
    /// Please remember to call Scene.Update(gameTime)!
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsConsoleActive && IsConsoleEnabled)
            Console.Update(gameTime);
    }

    protected void DrawConsole()
        => DrawConsole(SpriteBatch);

    protected void DrawConsole(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        if (ConsoleVisible)
            Console.Draw(SpriteBatch);
        spriteBatch.End();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        // Initialize the Textures of objects from MonoUtils
        Global.Initialize(Content);
    }

    protected void ToggleConsole()
    {
        if (IsConsoleEnabled)
            IsConsoleActive = !IsConsoleActive;
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
        if (!IsConsoleEnabled)
            return;

        if (Console is null)
            return;

        if (!IsConsoleActive)
            return;

        Console.TextInput(sender, e);
    }
}