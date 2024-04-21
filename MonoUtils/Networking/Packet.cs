using System.Reflection;
using System.Text;
using MonoUtils.Logging;

namespace MonoUtils.Networking;

public class Packet : IPacket
{
    private readonly IPacket _packet;


    public Packet(IPacket packet)
    {
        _packet = packet;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(0);
        writer.Write(_packet.GetType().FullName!);
        _packet.Write(writer);
    }

    public static IPacket Parse(BinaryReader reader)
    {
        var className = reader.ReadString();
        Type type = Type.GetType(className);

        if (type is null)
            throw new InvalidCastException("Unknown Paket type: " + className);
        MethodInfo parseMethod = type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
        IPacket packet = (IPacket)parseMethod.Invoke(null, new object[] { reader });
        return packet;
    }
}