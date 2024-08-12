using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Ui;

namespace MonoUtils;

public class Scene
{
    public readonly GraphicsDevice GraphicsDevice;
    public Camera Camera { get; private set; }
    public Display Display { get; private set; }

    public Scene(GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;
        Display = new Display(graphicsDevice);
        // Calculate the current screen onces
        Display.Update();
        Camera = new Camera(Display);
    }

    public void Update(GameTime gameTime)
    {
        Display.Update();
        Camera.Update(gameTime);
    }
}