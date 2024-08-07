using Microsoft.Xna.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoUtils.Logic;

public sealed class PositionCalculator
{

    private IMoveable _moveable;
    private readonly Vector2 _areaPosition;
    private readonly Vector2 _areaSize;
    private Vector2 _calculatedPosition;


    public PositionCalculator(Rectangle area) :
        this(area.Location.ToVector2(), area.Size.ToVector2(), null)
    {
    }

    public PositionCalculator(Rectangle area, IMoveable gameObject) :
        this(area.Location.ToVector2(), area.Size.ToVector2(), gameObject)
    {
    }

    public PositionCalculator(Vector2 areaPosition, Vector2 areaSize, IMoveable moveable)
    {
        _moveable = moveable;
        _calculatedPosition = areaPosition;
        _areaPosition = areaPosition;
        _areaSize = areaSize;
    }

    public PositionCalculator OnX(int on, int from)
    {
        _calculatedPosition.X = _areaPosition.X + _areaSize.X / from * on;
        return this;
    }
    
    public PositionCalculator OnX(float percent)
    {
        _calculatedPosition.X = _areaPosition.X + _areaSize.X * percent;
        return this;
    }
    
    public PositionCalculator OnY(int on, int from)
    {
        _calculatedPosition.Y = _areaPosition.Y + _areaSize.Y / from * on;
        return this;
    }
    
    public PositionCalculator OnY(float percent)
    {
        _calculatedPosition.Y = _areaPosition.Y + _areaSize.Y * percent;
        return this;
    }

    public PositionCalculator OnCenter()
    {
        _calculatedPosition = _areaPosition + _areaSize / 2;
        return this;
    }

    public PositionCalculator OnPositionX(float x)
    {
        _calculatedPosition.X = x;
        return this;
    }

    public PositionCalculator OnPositionY(float y)
    {
        _calculatedPosition.Y = y;
        return this;
    }
    
    public PositionCalculator Centered()
    {
        _calculatedPosition -= _moveable.GetSize() / 2;
        return this;
    }
    
    public PositionCalculator BySize(float percent)
    {
        _calculatedPosition += _moveable.GetSize() * percent;
        return this;
    }
    
    public PositionCalculator BySizeX(float percent)
    {
        _calculatedPosition.X += (_moveable.GetSize() * percent).X;
        return this;
    }
    
    public PositionCalculator BySizeY(float percent)
    {
        _calculatedPosition.Y += (_moveable.GetSize() * percent).Y;
        return this;
    }

    public PositionCalculator ByGrid(float x, float y)
    {
        _calculatedPosition += new Vector2(_areaSize.X * x, _areaSize.Y * y);
        return this;
    }
    
    public PositionCalculator ByGridX(float x)
    {
        _calculatedPosition += new Vector2(_areaSize.X * x, 0);
        return this;
    }
    
    public PositionCalculator ByGridY(float y)
    {
        _calculatedPosition += new Vector2(0, _areaSize.Y * y);
        return this;
    }

    public PositionCalculator WithX(float x)
    {
        _calculatedPosition += new Vector2(x, 0);
        return this;
    }

    public PositionCalculator WithY(float y)
    {
        _calculatedPosition += new Vector2(0, y);
        return this;
    }

    public PositionCalculator With(float x, float y)
    {
        _calculatedPosition += new Vector2(x, y);
        return this;
    }

    public void Move()
    {
        if (_moveable is null)
            return;
        _moveable.Move(_calculatedPosition);
    }


    public Vector2 Calculate()
        => _calculatedPosition;
}