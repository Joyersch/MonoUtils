using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;

namespace MonoUtils.Logic.Hitboxes.Collision;

public class CollisionResolver : IManageable
{
    private readonly Camera _camera;
    private List<VelocityAdapter> _dynamics;
    private List<GameObject> _stationaries;
    private HitboxCollection _hitboxCollection;

    public CollisionResolver(Camera camera)
    {
        _camera = camera;
        _dynamics = new();
        _stationaries = new();
        _hitboxCollection = new();
    }

    public Rectangle Rectangle { get; private set; }

    public void Update(GameTime gameTime)
    {
        Rectangle = _camera.Rectangle;
        foreach (var stationary in _stationaries)
            stationary.Update(gameTime);
        foreach (var dynamic in _dynamics)
        {
            dynamic.UpdateInteraction(gameTime, _hitboxCollection);
            dynamic.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var stationary in _stationaries)
        {
            if (stationary.Rectangle.Intersects(_camera.Rectangle))
                stationary.Draw(spriteBatch);
        }

        foreach (var dynamic in _dynamics)
        {
            if (dynamic.Rectangle.Intersects(_camera.Rectangle))
                dynamic.Draw(spriteBatch);
        }
    }

    public void AddDynamic(VelocityAdapter dynamic)
    {
        _dynamics.Add(dynamic);
    }

    public void AddStationary(GameObject stationary)
    {
        _stationaries.Add(stationary);
        _hitboxCollection.Add(stationary);
    }
}