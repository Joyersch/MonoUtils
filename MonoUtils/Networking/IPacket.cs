namespace MonoUtils.Networking;

public interface IPacket
{
    public void Write(BinaryWriter writer);
}