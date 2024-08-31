using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logging;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Logic;

/// <summary>
/// Uses LINQ under the hood. May need performance improvements later.
/// </summary>
public class CameraTripod : IManageable
{
    private readonly Camera _camera;

    public Rectangle Rectangle { get; private set; }

    private OverTimeMover _cameraMover;
    public readonly List<TripodPoint> _anchors;

    private readonly List<(int, int, float)> _angles;

    private int _position;

    public CameraTripod(Camera camera, OverTimeMover.MoveMode mode)
    {
        _camera = camera;
        _angles = [];
        _anchors =
        [
            new TripodPoint(camera.Position, camera.Zoom)
        ];
        _position = 0;
        _cameraMover = new OverTimeMover(camera, camera.Position, camera.ZoomSpeed, mode);
    }

    /// <summary>
    /// Set a point to the available positions for the camera to move to.
    /// </summary>
    /// <param name="anchor">Point where the Camera can move to</param>
    public void AddAnchor(TripodPoint anchor)
    {
        _anchors.Add(anchor);
        Calculate();
    }

    /// <summary>
    /// Create a new table for angles between anchors. Will be stored on _angles.
    /// </summary>
    private void Calculate()
    {
        _angles.Clear();
        for (var i = 0; i < _anchors.Count; i++)
        {
            for (var j = 0; j < _anchors.Count; j++)
            {
                if (i == j)
                    continue;

                var radian = MathHelper.Pi + Vector2Helper.GetAngle(_anchors[i].Position, _anchors[j].Position);
                var angle = MathHelper.ToDegrees(radian);
                _angles.Add((i, j, angle));
            }
        }
    }

    public void Transition(float angle)
    {
        if (_anchors.Count == 0)
            return;

        // Calculate the anchor with the closed angle to the given angle
        var toCheck = _angles.Where(a => a.Item1 == _position)
            .Select(a => (a.Item2, a.Item3, Vector2.Distance(_anchors[_position].Position, _anchors[a.Item2].Position)))
            .ToList();

        float lowestDifference = 360;
        float tolerance = 11.2575F;
        float lowestDistance = float.MaxValue;
        int index = -1;
        foreach (var check in toCheck)
        {
            if (check.Item1 == _position)
                continue;
            float difference = Math.Abs(check.Item2 - angle);
            difference = Math.Min(difference, 360F - difference);

            bool isLowestDifference = difference < lowestDifference;
            bool isInTolerance = difference < tolerance;
            bool isLowestDistance = check.Item3 < lowestDistance;

            if (isLowestDifference && isInTolerance && isLowestDistance)
            {
                index = check.Item1;
                lowestDifference = difference;
                lowestDistance = check.Item3;
            }
        }

        if (index == -1)
            return;


        // Move camera to the calculated anchor
        TripodPoint anchor = _anchors[index];
        // technically stop is not needed here, but I added it anyway >:3
        _cameraMover.Stop();
        _cameraMover.ChangeDestination(anchor.Position);
        _cameraMover.Start();
        _position = index;

        _camera.SetZoom(anchor.Zoom);
    }

    public void Update(GameTime gameTime)
    {
        _cameraMover.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Nothing to draw
    }
}