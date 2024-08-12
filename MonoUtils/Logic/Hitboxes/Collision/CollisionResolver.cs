using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;

namespace MonoUtils.Logic.Hitboxes.Collision;

public class CollisionResolver<T> : IManageable where T : IMoveable, IManageable, IHitbox
{
    private readonly Camera _camera;
    private List<VelocityAdapter<T>> _dynamics;
    private List<T> _stationaries;
    private HitboxCollection _hitboxCollection;

    public bool ManageStationaryUpdate { get; set; } = true;

    public bool ManageStationaryDraw { get; set; } = true;

    public bool ManageDynamicUpdate { get; set; } = true;

    public bool ManageDynamicDraw { get; set; } = true;


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
        if (ManageStationaryUpdate)
            foreach (var stationary in _stationaries)
                stationary.Update(gameTime);
        foreach (var dynamic in _dynamics)
        {
            dynamic.UpdateInteraction(gameTime, _hitboxCollection);
            if (ManageDynamicUpdate)
                dynamic.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (ManageStationaryDraw)
            foreach (var stationary in _stationaries.Where(stationary =>
                         stationary.Rectangle.Intersects(_camera.Rectangle)))
            {
                stationary.Draw(spriteBatch);
            }

        if (!ManageDynamicDraw)
            return;

        foreach (var dynamic in _dynamics.Where(dynamic =>
                     dynamic.Rectangle.Intersects(_camera.Rectangle)))
        {
            dynamic.Draw(spriteBatch);
        }
    }

    public void AddDynamic(VelocityAdapter<T> dynamic)
    {
        _dynamics.Add(dynamic);
    }

    public void AddStationary(T stationary)
    {
        _stationaries.Add(stationary);
        _hitboxCollection.Add(stationary);
    }
}