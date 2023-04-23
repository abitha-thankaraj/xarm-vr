using UnityEngine;
using NetMQ;
using AsyncIO;

public class ControllerTracking : MonoBehaviour
{
    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    private bool startRecording;
    private ZMQClient zmqClient;

    [SerializeField]
    private string tcpAddress = "tcp://10.19.216.156:5555";

    private void Start()
    {
        Logger.Log("Init libs");
        ForceDotNet.Force();
        zmqClient = new ZMQClient(tcpAddress);
        startRecording = false;
    }

    private void Update()
    {
        Logger.Log("Hello world!");

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            Logger.Log("A button pressed. Start recording");
            startRecording = true;

            string message = "start:" + getState();
            zmqClient.SendMessageToServer(message);
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            Logger.Log("B button pressed. Stop recording");
            startRecording = false;
            string message = "stop";
            zmqClient.SendMessageToServer(message);
        }

        else if (startRecording)
        {
            zmqClient.SendMessageToServer(getState());

        }

        else
        {
            Logger.Log("Press A on right controller to start recording; Press B to stop.");
        }
    }

    private void OnDestroy()
    {
        Disconnect();
        NetMQConfig.Cleanup();
    }

    public void Disconnect()
    {
        zmqClient.Dispose();
        zmqClient = null;
    }

    public void Connect()
    {
        if (zmqClient == null)
        {
            zmqClient = new ZMQClient(tcpAddress);
        }
    }

    private string getState() {

        Vector3 leftPosition = OVRInput.GetLocalControllerPosition(leftController);
        Vector3 rightPosition = OVRInput.GetLocalControllerPosition(rightController);
        Quaternion leftRotation = OVRInput.GetLocalControllerRotation(leftController);
        Quaternion rightRotation = OVRInput.GetLocalControllerRotation(rightController);

        Logger.Log(leftPosition.ToString());
        Logger.Log(leftRotation.ToString());
        Logger.Log(rightPosition.ToString());
        Logger.Log(rightRotation.ToString());

        string state = $"{leftPosition.x},{leftPosition.y},{leftPosition.z}," +
            $"{leftRotation.x},{leftRotation.y},{leftRotation.z},{leftRotation.w}" +
            $"|{rightPosition.x},{rightPosition.y},{rightPosition.z}," +
            $"{rightRotation.x},{rightRotation.y},{rightRotation.z},{rightRotation.w}";

        return state;

    }
}



//using UnityEngine;
//using UnityEngine.UI;
//using NetMQ;
//using NetMQ.Sockets;
//using AsyncIO;


//public class ControllerTracking : MonoBehaviour
//{

//    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
//    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;
//    private bool startRecording;

//    private RequestSocket client; //TODO: Decouple this.

//    void Start()
//    {
//        // Initialize the libraries
//        Logger.Log("Init libs");
//        ForceDotNet.Force();
//        CreateClient();
//        startRecording = false;
//    }

//    void CreateClient()
//    {
//        client = new RequestSocket();
//        client.Connect("tcp://10.19.216.156:5555");
//        //client.Connect("tcp://192.168.0.35:5555");
//        Logger.Log("Connected");
//    }

//    void Update()
//    {
//        Logger.Log("Hello world!");


//        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
//        {
//            Logger.Log("A button pressed. Start recording");
//            startRecording = true;
//            SendStartMessageToServer();
//        }


//        else if (startRecording)
//        {
//            Vector3 leftPosition = OVRInput.GetLocalControllerPosition(leftController);
//            Vector3 rightPosition = OVRInput.GetLocalControllerPosition(rightController);
//            Quaternion leftRotation = OVRInput.GetLocalControllerRotation(leftController);
//            Quaternion rightRotation = OVRInput.GetLocalControllerRotation(rightController);

//            Logger.Log(leftPosition.ToString());
//            Logger.Log(leftRotation.ToString());
//            Logger.Log(rightPosition.ToString());
//            Logger.Log(rightRotation.ToString());


//            SendMessageToServer(leftPosition, leftRotation, rightPosition, rightRotation);
//        }
//        else
//        {
//            Logger.Log("Press A on right controller to start recording");
//        }
//    }

//    void SendMessageToServer(Vector3 leftPosition, Quaternion leftRotation, Vector3 rightPosition, Quaternion rightRotation)
//    {
//        string message = $"{leftPosition.x},{leftPosition.y},{leftPosition.z},{leftRotation.x},{leftRotation.y},{leftRotation.z},{leftRotation.w}|{rightPosition.x},{rightPosition.y},{rightPosition.z},{rightRotation.x},{rightRotation.y},{rightRotation.z},{rightRotation.w}";
//        client.SendFrame(message);

//        string response = client.ReceiveFrameString();
//        Logger.Log("Received response: " + response);
//    }

//    void SendStartMessageToServer()
//    {
//        string message = "start";
//        client.SendFrame(message);

//        string response = client.ReceiveFrameString();
//        Logger.Log("Received response: " + response);
//    }

//    private void OnDestroy()
//    {
//        client.Close();
//        client.Dispose();
//        NetMQConfig.Cleanup();
//    }
//}
