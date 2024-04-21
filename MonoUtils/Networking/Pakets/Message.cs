namespace MonoUtils.Networking.Pakets;

public class Message : IPacket
{
    public string Value { get; private set; }

    public Message(string value)
    {
        Value = value;
    }

    public static IPacket Parse( BinaryReader reader)
    {
        string message = reader.ReadString();
        return new Message(message);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Value);
    }
}