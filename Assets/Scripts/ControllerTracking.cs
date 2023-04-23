//using UnityEngine;

//public class ControllerTracking : MonoBehaviour
//{
//    public WebSocketClient webSocketClient;

//    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
//    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;

//    void Update()
//    {
//        Vector3 leftPosition = OVRInput.GetLocalControllerPosition(leftController);
//        Vector3 rightPosition = OVRInput.GetLocalControllerPosition(rightController);

//        Quaternion leftRotation = OVRInput.GetLocalControllerRotation(leftController);
//        Quaternion rightRotation = OVRInput.GetLocalControllerRotation(rightController);

//        string message = JsonUtility.ToJson(new
//        {
//            left_position = leftPosition.ToString(),
//            right_position = rightPosition.ToString(),
//            left_rotation = leftRotation.ToString(),
//            right_rotation = rightRotation.ToString()
//        });

//        webSocketClient.SendMessage(message);
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using NetMQ;
using NetMQ.Sockets;
using AsyncIO;


public class ControllerTracking : MonoBehaviour
{
 
    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    private bool startRecording;

    private RequestSocket client; //TODO: Decouple this.

    void Start()
    {
        // Initialize the libraries
        Logger.Log("Init libs");
        ForceDotNet.Force();
        CreateClient();
        startRecording = false;
    }

    void CreateClient()
    {
        client = new RequestSocket();
        client.Connect("tcp://10.19.216.156:5555");
        //client.Connect("tcp://192.168.0.35:5555");
        Logger.Log("Connected");
    }

    void Update()
    {
        Logger.Log("Hello world!");


        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            Logger.Log("A button pressed. Start recording");
            startRecording = true;
            SendStartMessageToServer();
        }


        else if (startRecording)
        {
            Vector3 leftPosition = OVRInput.GetLocalControllerPosition(leftController);
            Vector3 rightPosition = OVRInput.GetLocalControllerPosition(rightController);
            Quaternion leftRotation = OVRInput.GetLocalControllerRotation(leftController);
            Quaternion rightRotation = OVRInput.GetLocalControllerRotation(rightController);

            Logger.Log(leftPosition.ToString());
            Logger.Log(leftRotation.ToString());
            Logger.Log(rightPosition.ToString());
            Logger.Log(rightRotation.ToString());


            SendMessageToServer(leftPosition, leftRotation, rightPosition, rightRotation);
        }
        else
        {
            Logger.Log("Press A on right controller to start recording");
        }
    }

    void SendMessageToServer(Vector3 leftPosition, Quaternion leftRotation, Vector3 rightPosition, Quaternion rightRotation)
    {
        string message = $"{leftPosition.x},{leftPosition.y},{leftPosition.z},{leftRotation.x},{leftRotation.y},{leftRotation.z},{leftRotation.w}|{rightPosition.x},{rightPosition.y},{rightPosition.z},{rightRotation.x},{rightRotation.y},{rightRotation.z},{rightRotation.w}";
        client.SendFrame(message);

        string response = client.ReceiveFrameString();
        Logger.Log("Received response: " + response);
    }

    void SendStartMessageToServer()
    {
        string message = "start";
        client.SendFrame(message);

        string response = client.ReceiveFrameString();
        Logger.Log("Received response: " + response);
    }

    private void OnDestroy()
    {
        client.Close();
        client.Dispose();
        NetMQConfig.Cleanup();
    }
}
