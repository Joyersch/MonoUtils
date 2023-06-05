using Microsoft.Xna.Framework;

namespace MonoUtils.Logic.Listener;

public class RelativePositionListener
{
    private readonly List<PositionData> _mappings;



    public RelativePositionListener()
    {
        _mappings = new List<PositionData>();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var valueTuple in _mappings)
        {
            var position = valueTuple.Main.GetPosition();
            if (position != valueTuple.OldPosition)
            {
                var subPosition = valueTuple.Sub.GetPosition();
                var newPosition = subPosition + (position - valueTuple.OldPosition);
                valueTuple.Sub.Move(newPosition);
            }

            valueTuple.OldPosition = position;
        }
    }

    public void Add(IMoveable main, IMoveable sub)
        => _mappings.Add(new PositionData(main, main.GetPosition(), sub));
}