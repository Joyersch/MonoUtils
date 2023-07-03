using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Management;

namespace MonoUtils.Logic.Hitboxes.Collision;

public class VelocityAdapter : IManageable, IInteractable, IHitbox
{
    public Vector2 Velocity { get; private set; }
    public readonly GameObject Object;

    public VelocityAdapter(GameObject @object)
    {
        Object = @object;
    }

    public Rectangle Rectangle => Object.Rectangle;

    public void Update(GameTime gameTime)
    {
        Object.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Object.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        var time = (float) gameTime.ElapsedGameTime.TotalMinutes;
        var hitboxes = toCheck.Hitbox.ToList();
        var velocityRange = Rectangle.Union(Object.Rectangle
            , new Rectangle((Object.Position + Velocity).ToPoint(), Object.Rectangle.Size));
        
        var inRange = hitboxes.Where(h => h.Intersects(velocityRange)).ToList();
        
        //sort hitboxes by distance
        inRange.Sort((a, b)
            => (int) (Vector2.Distance(Object.Rectangle.Center.ToVector2(), a.Center.ToVector2()) -
                      Vector2.Distance(Object.Rectangle.Center.ToVector2(), b.Center.ToVector2())));
        
        foreach (var hitbox in inRange)
        {
            if (Rectangles.DynamicRectangleVsRectangle(Object.Rectangle, Velocity, time, hitbox,
                    out Vector2 contactPoint,
                    out Vector2 ContactNormal, out float ContactTime))
            {
                Velocity += ContactNormal *
                            new Vector2(Math.Abs(Velocity.X), Math.Abs(Velocity.Y)) *
                            (1 - ContactTime);
            }
        }

        Object.Move(Object.Position + Velocity * time);
    }

    public Rectangle[] Hitbox => Object.Hitbox;

    public void SetVelocity(Vector2 newVelocity)
        => Velocity = newVelocity;

    public void AddVelocity(Vector2 newVelocity)
        => Velocity += newVelocity;
}