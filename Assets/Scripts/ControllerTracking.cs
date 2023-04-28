using UnityEngine;
using NetMQ;
using AsyncIO;
using System.Collections;
using OVR;
using System;

public class ControllerTracking : MonoBehaviour
{
    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    private bool startRecording;
    private ZMQClient zmqClient;
    private ControllerState controllerState; 

    [SerializeField]
    private string tcpAddress = "tcp://10.19.216.156:5555";

    private void Start()
    {
        Logger.Log("Init libs");
        ForceDotNet.Force();
        zmqClient = new ZMQClient(tcpAddress);
        controllerState = new ControllerState(leftController, rightController);

        //Connect();
        //startRecording = false;
    }

    private void Update()
    {
        controllerState.UpdateState();
        string state = controllerState.ToString();
        Logger.Log(state);
        zmqClient.SendMessageToServer(controllerState.ToString());
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

    }
