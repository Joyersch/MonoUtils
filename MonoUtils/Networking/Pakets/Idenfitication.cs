namespace MonoUtils.Networking.Pakets;

public class Idenfitication : IPacket
{
    public Guid Id { get; private set; }

    public Idenfitication(Guid id)
    {
        Id = id;
    }
    public static IPacket Parse(BinaryReader reader)
    {
        byte[] uuidBytes = reader.ReadBytes(16);
        Guid uuid = new Guid(uuidBytes);
        return new Idenfitication(uuid);
    }
    public void Write( BinaryWriter writer )
    {
        writer.Write(Id.ToByteArray());
    }
}