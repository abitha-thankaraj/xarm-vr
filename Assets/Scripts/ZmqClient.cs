
using UnityEngine;
using UnityEngine.UI;
using NetMQ;
using NetMQ.Sockets;
using AsyncIO;
using System;

public class ZMQClient
{
    private RequestSocket client;
    private string tcpAddress;

    public ZMQClient(string tcpAddress)
    {
        this.tcpAddress = tcpAddress;
        Connect();
    }


    public void SendMessageToServer(string message)
    {
        if (client == null || client.IsDisposed || client.Options.Linger < TimeSpan.Zero)
        {
            Reconnect();
        }

        client.SendFrame(message);

        string response = client.ReceiveFrameString();
        Logger.Log("Received response: " + response);
    }

    public void Dispose()
    {
        client?.Dispose();
    }

    private void Connect()
    {
        try
        {
            if (client == null || client.IsDisposed || client.Options.Linger < TimeSpan.Zero)
            {
                client = new RequestSocket();
                client.Connect(tcpAddress);
            }
        }
        catch (Exception e)
        {
            Logger.Log("Error connecting to server: " + e.Message);
        }
    }

    private void Reconnect()
    {
        Logger.Log("Connection lost. Attempting to reconnect...");
        client?.Dispose();
        Connect();
    }
}
