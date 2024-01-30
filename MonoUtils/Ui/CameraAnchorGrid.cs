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

    public event Action StoppedMoving;

    private GameObject[] _anchors;

    public bool IsDraw = false;

    public CameraAnchorGrid(Camera camera, IMoveable indicator, float timeToMove, OverTimeMover.MoveMode moveMode)
    {
        _camera = camera;
        _indicator = indicator;
        _mover = new OverTimeMover(camera, Vector2.Zero, timeToMove, moveMode);
        _mover.ArrivedOnDestination += CalculateAnchors;
        _mover.ArrivedOnDestination += delegate { StoppedMoving?.Invoke(); };
        CalculateAnchors();
    }

    public Rectangle Rectangle => _camera.Rectangle;

    public void Update(GameTime gameTime)
    {
        _mover.Update(gameTime);

        for (int i = 0; i < 9; i++)
        {
            _anchors[i].Update(gameTime);
        }

        if (_mover.IsMoving)
            return;

        if (!IsIndicatorCloserToOuterAnchor(-32F, out int index))
            return;

        _mover.ChangeDestination(_anchors[index].Rectangle.Center.ToVector2());
        _mover.Start();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsDraw)
            return;

        for (int i = 0; i < 9; i++)
        {
            _anchors[i].Draw(spriteBatch);
        }
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
                GameObject anchor = _anchors[i] ??= new GameObject(Vector2.Zero, Vector2.One * 16);
                anchor.GetCalculator(_camera.Rectangle)
                    .OnCenter()
                    .Centered()
                    .ByGrid(x, y)
                    .Move();
                i++;
            }
        }
    }

    private bool IsIndicatorCloserToOuterAnchor(float tolerance, out int id)
    {
        id = 4;

        Vector2 indicatorPosition = _indicator.GetPosition();
        Vector2 centerPosition = _anchors[4].Position;

        float distanceToCenter = Vector2.Distance(indicatorPosition, centerPosition);

        float closestDistance = float.MaxValue;

        for (int i = 0; i < _anchors.Length; i++)
        {
            if (i == 4)
                continue;

            Vector2 anchorPosition = _anchors[i].Position;
            float distance = Vector2.Distance(indicatorPosition, anchorPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                id = i;
            }
        }

        return distanceToCenter - tolerance > closestDistance;
    }
}