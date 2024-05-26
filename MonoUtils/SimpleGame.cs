using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Console;
using MonoUtils.Logging;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;

namespace MonoUtils;

public class SimpleGame : Game
{
    protected readonly GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch;

    protected Display Display;
    protected SettingsAndSaveManager SettingsAndSaveManager;

    protected DevConsole Console;
    protected bool IsConsoleActive;
    protected bool IsConsoleEnabled;

    protected string SaveDirectory = "saves";
    protected int? SaveFile = null;

    protected bool Debug;

    protected string[] Args;

    public SimpleGame()
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

        Display = new Display(GraphicsDevice);
        Window.TextInput += OnTextInput;

        Global.CommandProcessor.Initialize();

        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        Console = new DevConsole(Global.CommandProcessor, Vector2.Zero, Display.SimpleScale,
            Console);
        Log.Out = new LogAdapter(Console);

        SettingsAndSaveManager = new SettingsAndSaveManager(SaveDirectory);
        SettingsAndSaveManager.SetSaveFile(SaveFile);

        if (!SettingsAndSaveManager.LoadSettings())
            SettingsAndSaveManager.SaveSettings();

        if (SaveFile is not null && !SettingsAndSaveManager.LoadSaves())
            SettingsAndSaveManager.SaveSave();
        
        TextProvider.Initialize();
    }
    
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Display.Update();

        if (IsConsoleActive && IsConsoleEnabled)
            Console.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    protected void DrawConsole()
    {
        SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        if (IsConsoleActive && IsConsoleEnabled)
            Console.Draw(SpriteBatch);
        SpriteBatch.End();
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