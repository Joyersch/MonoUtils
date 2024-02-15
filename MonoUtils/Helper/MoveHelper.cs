using Microsoft.Xna.Framework;
using MonoUtils.Logic;

namespace MonoUtils.Helper;

public class MoveHelper
{
    public static void MoveTowards(IMoveable move, IMoveable to, float distance)
    {
        var movePosition = move.GetPosition();
        var moveSize = move.GetSize();

        var moveFrom = movePosition + moveSize / 2;

        var toPosition = to.GetPosition();
        var toSize = to.GetSize();

        var moveTo = toPosition + toSize / 2;

        var position = Vector2.Normalize(moveTo - moveFrom);

        move.Move(movePosition + position * distance);
    }

    public static void RotateTowards(IRotateable rotate, IMoveable towards)
    {
        var rotatePosition = rotate.GetPosition();
        var rotateSize = rotate.GetSize();

        var rotateCenter = rotatePosition + rotateSize / 2;

        var towardsPosition = towards.GetPosition();
        var towardsSize = towards.GetSize();

        var towardsCenter = towardsPosition + towardsSize / 2;

        var direction = towardsCenter - rotateCenter;
        rotate.Rotation = (float)(Math.Atan2(direction.Y, direction.X));

    }
}