using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Logic;

/// <summary>
/// WIP! DO NOT USE!
/// </summary>
public class CameraTripod : IManageable
{
    private readonly Camera _camera;

    public Rectangle Rectangle { get; private set; }

    private OverTimeMover _cameraMover;
    public readonly List<TripodPoint> _anchors;

    private readonly Dictionary<int, float> _angles;

    private int _position;

    public CameraTripod(Camera camera, OverTimeMover.MoveMode mode)
    {
        _camera = camera;
        _cameraMover = new OverTimeMover(camera, camera.Position, camera.ZoomSpeed, mode);
    }

    public void AddAnchor(TripodPoint anchor)
    {
        foreach (var point in _anchors)
            _angles.Add(_anchors.Count, Vector2Helper.GetAngle(point.Position, anchor.Position));

        _anchors.Add(anchor);
    }

    public void Calcutate()
    {
        _angles.Clear();
        for (var i = 0; i < _anchors.Count; i++)
        {
            for (var j = 0; j < _anchors.Count; j++)
            {
                if (i == j)
                    continue;
                _angles.Add(i, Vector2Helper.GetAngle(_anchors[i].Position, _anchors[j].Position));
            }
        }
    }

    public void Transition(float angle)
    {

        // this will call determine the Point to go to and then give it to MoveToAnchor
    }


    private void MoveToAnchor(int index)
    {
        _position = index;
        TripodPoint anchor = _anchors[index];
        // technically stop is not needed here, but I added it anyway >:3
        _cameraMover.Stop();
        _cameraMover.ChangeDestination(anchor.Position);
        _cameraMover.Start();

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