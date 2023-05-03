using UnityEngine;
using NetMQ;
using AsyncIO;
using System.Collections;
using OVR;
using System;
using NetMQ.Sockets;

public class ControllerTracking : MonoBehaviour
{
    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    private ControllerState controllerState;


    private PublisherSocket publisher;
    private string tcpAddress = "tcp://*:5555";

    void Start()
    {
        Logger.Log("Init libs");
        ForceDotNet.Force();
        controllerState = new ControllerState(leftController, rightController);

        Logger.Log("Create publisher");
        // Create a new publisher socket and bind it to the TCP address
        publisher = new PublisherSocket();
        publisher.Bind(tcpAddress);

        // Start publishing the Oculus controller state
        //StartCoroutine(PublishOculusControllerState());
    }


    void Update() {
        controllerState.UpdateState();
        string state = controllerState.ToString();
        Logger.Log(state);
        // Publish the message on the "oculus_controller" topic
        publisher.SendMoreFrame("oculus_controller").SendFrame(state);

    }


    void OnDestroy()
    {
        // Dispose of the publisher socket when the script is destroyed
        publisher?.Dispose();
    }


}



//public class ControllerTracking : MonoBehaviour
//{
//    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
//    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;

//    private bool startRecording;
//    private ZMQClient zmqClient;
//    private ControllerState controllerState;

//    [SerializeField]
//    private string tcpAddress = "tcp://10.19.216.156:5555";

//    private void Start()
//    {
//        Logger.Log("Init libs");
//        ForceDotNet.Force();
//        zmqClient = new ZMQClient(tcpAddress);
//        controllerState = new ControllerState(leftController, rightController);

//    }

//    private void Update()
//    {


//        controllerState.UpdateState();
//        string state = controllerState.ToString();
//        Logger.Log(state);
//        // TODO : move all try catch here
//        try

//        {
//            zmqClient.SendMessageToServer(controllerState.ToString());
//        }

//        catch (Exception e) {
//            Logger.Log(e.ToString());
//        }

//    }

//    private void OnDestroy()
//    {
//        Disconnect();
//        NetMQConfig.Cleanup();
//    }

//    public void Disconnect()
//    {
//        zmqClient.Dispose();
//        zmqClient = null;
//    }

//    public void Connect()
//    {
//        if (zmqClient == null)
//        {
//            zmqClient = new ZMQClient(tcpAddress);
//        }
//    }

//}
