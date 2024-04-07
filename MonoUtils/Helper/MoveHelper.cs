﻿using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes.Collision;

namespace MonoUtils.Helper;

public static class MoveHelper
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
        rotate.Rotation = GetAngle(rotate, towards);
    }

    public static float GetAngle(IRotateable origin, IMoveable destination)
    {
        var rotatePosition = origin.GetPosition();
        var rotateSize = origin.GetSize();

        var rotateCenter = rotatePosition + rotateSize / 2;

        var towardsPosition = destination.GetPosition();
        var towardsSize = destination.GetSize();

        var towardsCenter = towardsPosition + towardsSize / 2;
        var direction = towardsCenter - rotateCenter;
        return (float)Math.Atan2(direction.Y, direction.X);
    }

    /// <summary>
    /// to - from
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Vector2 GetRelative(Vector2 from, Vector2 to)
        => to - from;

    public static Rectangle GetRectangle(this IMoveable moveable)
        => new Rectangle(moveable.GetPosition().ToPoint(), moveable.GetSize().ToPoint());
}