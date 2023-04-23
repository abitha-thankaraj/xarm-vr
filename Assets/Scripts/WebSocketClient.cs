using UnityEngine;
using WSSharp = WebSocketSharp;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    public string serverAddress = "ws://192.168.0.35:8080";

    private WSSharp.WebSocket webSocket;

    void Start()
    {
        webSocket = new WSSharp.WebSocket(serverAddress);
        webSocket.OnMessage += OnWebSocketMessage;
        webSocket.ConnectAsync();
    }

    void OnDestroy()
    {
        webSocket.CloseAsync();
    }

    private void OnWebSocketMessage(object sender, WSSharp.MessageEventArgs e)
    {
        Debug.Log("Received message: " + e.Data);
    }
}
