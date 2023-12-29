using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Logic;

namespace MonoUtils.Ui;

public class CameraAnchorGrid : IManageable
{
    private readonly Camera _camera;
    private readonly IMoveable _indicator;
    private readonly OverTimeMover _mover;

    private GameObject[] _anchors;
    private int _closeAnchorId = 4; // 4 should always be the one on the camera position

    public CameraAnchorGrid(Camera camera, IMoveable indicator, float timeToMove, OverTimeMover.MoveMode moveMode)
    {
        _camera = camera;
        _indicator = indicator;
        _mover = new OverTimeMover(camera, Vector2.Zero, timeToMove, moveMode);
        _mover.ArrivedOnDestination += CalculateAnchors;
        CalculateAnchors();
    }

    public Rectangle Rectangle => _camera.Rectangle;

    public void Update(GameTime gameTime)
    {
        _mover.Update(gameTime);

        //if (_mover.IsMoving)
        //   return;

        int index = GetAnchorCloseToIndicator();
        if (index != _closeAnchorId)
        {
            _mover.ChangeDestination(_anchors[index].Position);
            _mover.Start();
            _closeAnchorId = index;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    private void CalculateAnchors()
    {
        _anchors ??= new GameObject[9];

        int i = 0;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                // Get the object at given position i or a new object if null
                GameObject anchor = _anchors[i] ?? new GameObject();
                anchor.GetCalculator(_camera.Rectangle)
                    .OnCenter()
                    .ByGrid(x, y)
                    .Move();
                _anchors[i] = anchor;
                i++;
            }
        }
    }

    private int GetAnchorCloseToIndicator()
    {
        Vector2 cameraPosition = _indicator.GetPosition();
        int closestAnchorIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < _anchors.Length; i++)
        {
            Vector2 anchorPosition = _anchors[i].Position;
            float distance = Vector2.Distance(cameraPosition, anchorPosition);

            if (distance >= closestDistance)
                continue;

            closestDistance = distance;
            closestAnchorIndex = i;
        }

        return closestAnchorIndex;
    }
}