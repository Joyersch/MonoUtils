﻿using System.Net.Sockets;
using System.Text;
using MonoUtils.Logging;
using MonoUtils.Networking.Pakets;

namespace MonoUtils.Networking;

public class Client : IDisposable
{
    private readonly string _url;
    private readonly int _port;
    private TcpClient _tcpClient;
    private Thread _connection;
    private Stream _stream;

    public bool IsConnected { get; private set; }

    public event Action<IPacket> RecievedMessage;
    public event Action Disconnected;
    public event Action Connected;


    public Client(TcpClient client)
    {
        _tcpClient = client;
        Connect();
    }

    public Client(string url, int port)
    {
        _url = url;
        _port = port;
        Connect();
    }

    public void Connect()
    {
        if (IsConnected)
            return;

        _tcpClient ??= new TcpClient(_url, _port);
        _stream = _tcpClient.GetStream();
        _connection = new Thread(ReadFromConnection);
        _connection.Start(_stream);
        IsConnected = true;
        Connected?.Invoke();
    }

    public void Disconnect()
    {
        if (!IsConnected)
            return;

        if (_connection.IsAlive)
            _connection.Interrupt();
        _connection = null;
        _stream.Close();
        _stream.Dispose();
        _stream = null;
        IsConnected = false;
        Disconnected?.Invoke();
    }

    public void Send(IPacket message)
    {
        if (!IsConnected)
            return;
        var packet = new Packet(message);
        BinaryWriter writer = new(_stream);
        packet.Write(writer);
    }

    private void ReadFromConnection(object? obj)
    {
        if (_stream is null)
            return;

        NetworkStream stream = (NetworkStream)_stream;
        byte[] receivedBuffer = new byte[10240];
        while (true)
        {
            try
            {
                int numberOfBytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                if (numberOfBytesRead == 0)
                {
                    Disconnect();
                    return;
                }

                BinaryReader reader = new BinaryReader(stream);
                var paket = Packet.Parse(reader);
                RecievedMessage?.Invoke(paket);
            }
            catch (Exception exception)
            {
                Log.WriteError(exception.Message);
                Disconnect();
                return;
            }
        }
    }

    public void Dispose()
    {
        _stream?.Dispose();
        _tcpClient.Dispose();
    }
}