using System.Net;
using System.Net.Sockets;

namespace MonoUtils.Networking;

public class ServerBroker
{
    private readonly int _port;
    private Thread _listenerThread;

    public event Action<Client> ClientConnected;

    public ServerBroker(int port)
    {
        _port = port;
    }

    public void Start()
    {
        _listenerThread = new Thread(ListenForClients);
        _listenerThread.Start(_port);
    }

    public void Stop()
    {
        _listenerThread = new Thread(ListenForClients);
        _listenerThread.Interrupt();
        _listenerThread = null;
    }

    private void ListenForClients(object? obj)
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

        while (true)
        {
            var connection = listener.AcceptTcpClient();
            ClientConnected?.Invoke(new Client(connection));
        }
    }
}