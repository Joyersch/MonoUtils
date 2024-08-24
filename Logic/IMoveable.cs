using Microsoft.Xna.Framework;
namespace MonoUtils.Logic;

public interface IMoveable : ISpatial
{

    public void Move(Vector2 newPosition);
}