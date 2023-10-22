namespace MonoUtils.Ui.Objects.Console;

public class Backlog : List<string>
{
    private int pointer;

    public void MovePointerUp()
        => pointer = pointer-- > 0 ? pointer : 0;

    public void MovePointerDown()
        => pointer = pointer++ < Count ? pointer : Count;
    
    public List<string> GetRangeFromPointer(int size)
    {
        int endIndex = Math.Min(pointer + size, Count);
        int startIndex = Math.Min(pointer, endIndex);
        return GetRange(startIndex, endIndex - startIndex);
    }

}