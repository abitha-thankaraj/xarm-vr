
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
//using UnityEngine;
//using UnityEngine.UI;
//using NetMQ;
//using NetMQ.Sockets;
//using AsyncIO;
//using System;

//public class ZMQClient
//{
//    private RequestSocket client;
//    private string tcpAddress;

//    public ZMQClient(string tcpAddress)
//    {
//        this.tcpAddress = tcpAddress;
//        Connect();
//    }


//    public void SendMessageToServer(string message)
//    {
//        try
//        {
//            if (client == null || client.IsDisposed || client.Options.Linger < TimeSpan.Zero)
//            {
//                Logger.Log("Attempting to reconnect");
//                Reconnect();
//            }
//        }
//        catch (Exception e) {
//            Logger.Log("Error reconnecting to server: " + e.Message);
//            client?.Dispose();
//            client = null;

//        }

//        // If no response; reconnect
//        try
//        {
//            bool send_flag = client.TrySendFrame(message);
//            if (!send_flag) {
//                Logger.Log("Unable to send request within time frame.");
//                throw new NetMQException("Did not send a request within the specified time frame.");


//            }

//            //client.SendFrame(message);

//            string response;
//            bool received_response_flag = client.TryReceiveFrameString(out response);

//            if (!received_response_flag || response == null)
//            {
//                Logger.Log("Receive responsde timed out");
//                throw new NetMQException("Did not receive a response within the specified time frame.");
//            }

//        }

//        catch (Exception e)
//        {

//            Logger.Log("Full error: " + e.ToString());
//            Logger.Log("Error connecting to server; during send receive: " + e.Message);
//            client?.Dispose();
//            client = null;
//        }


//    }

//    public void Dispose()
//    {
//        client?.Dispose();
//    }


//    public void Connect()
//    {
//        try
//        {
//            if (client == null || client.IsDisposed || client.Options.Linger < TimeSpan.Zero)
//            {
//                client = new RequestSocket();
//                client.Connect(tcpAddress);
//            }
//        }
//        catch (Exception e)
//        {
//            Logger.Log("Error connecting to server: " + e.Message);
//        }
//    }

//    public void Reconnect()
//    {
//        Logger.Log("Connection lost. Attempting to reconnect...");
//        client?.Dispose();
//        Connect();
//    }
//}
