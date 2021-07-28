    using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using Valve.VR;

public class GetControllerPos : MonoBehaviour
{
    //손하민학생 논문 연구용 데이터 추출 스크립트
    //추후 삭제될 수도 있고, 계속 사용할 수도 있음

/*    private List<InputDevice> _inputDevices = new List<InputDevice>();
    private bool _leftTriggerDown;
    private bool _leftGripDown;
    // and other left hand buttons

    private bool _rightTriggerDown;
    private bool _rightGripDown;*/
    // and other right hand buttons


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public Transform HEAD_Controller; //Vive머리 --- XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/CenterEyeAnchor
    public Transform Hand_L_Controller; //왼손 컨트롤러 --- XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor
    public Transform Hand_R_Controller; //오른손 컨트롤러 --- XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/RightHandAnchor/RightControllerAnchor
/*
    [Header("XR Simulated Controller")]
    public XRSimulatedController leftController;
    public XRSimulatedController rightController;*/

    /*int Click_L = 0; //왼손클릭
    int Click_R = 0; //오른손클릭*/

    float Timer = 0; //1초 체크용 타이머

    string path_Pos;
    /*string path_Hand_L;
    string path_Hand_R;*/

    FileStream PosInfo;
    StreamWriter PosWriter;

    /*FileStream LeftInfo;
    StreamWriter LeftWriter;

    FileStream RightInfo;
    StreamWriter RightWriter;*/

    string FilePath = Application.streamingAssetsPath + "/Hippo/";
    /*string SaveLeft;
    string SaveRight;*/

    public int frame;

    void Start()
    {
        string SaveTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        path_Pos = FilePath + "ControllerData" + "_" + SaveTime + "_Pos" + ".txt";
        /*path_Hand_L = FilePath + "ControllerData" + "_" + SaveTime + "_Left" + ".txt";
        path_Hand_R = FilePath + "ControllerData" + "_" + SaveTime + "_Right" + ".txt";*/

        /*SteamVR_Actions.vRIF_LeftTriggerDown.AddOnStateDownListener(TriggerLeftPressed, SteamVR_Input_Sources.Any);
        SteamVR_Actions.vRIF_RightTriggerDown.AddOnStateDownListener(TriggerRightPressed, SteamVR_Input_Sources.Any);*/

        /*SteamVR_Actions.vRIF_LeftTrigger.AddOnActiveChangeListener(TriggerLeftPressed, SteamVR_Input_Sources.Any);
        SteamVR_Actions.vRIF_RightTrigger.AddOnActiveChangeListener(TriggerRightPressed, SteamVR_Input_Sources.Any);

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, _inputDevices);
        InputDevices.GetDevices(_inputDevices);
        Debug.Log(_inputDevices.Count);*/

        /*while (_inputDevices.Count == 0)
        {
            Debug.Log("Get Device Function Called");
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, _inputDevices);
            Debug.Log(_inputDevices.Count);
        }*/

       /* StartCoroutine(GetControllers());*/
    }

   /* IEnumerator GetControllers()
    {
        Debug.Log("Waiting for Get Device Function to be Called...");
        yield return new WaitUntil(() => frame >= 10);
        Debug.Log("Get Device Function Called");
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, _inputDevices);
        Debug.Log(_inputDevices.Count);
        foreach (InputDevice inputDevice in _inputDevices)
        {
            Debug.Log(inputDevice.name);
            Debug.Log(inputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Left));
            Debug.Log(inputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Right));
            Debug.Log("1. "+_inputDevices[0]);
            Debug.Log("2. "+_inputDevices[1]);
        }
    }
*/
    void Update()
    {
        /*if (frame <= 10)
        {
            *//*Debug.Log("Frame: " + frame);*//*
            frame++;
        }

        foreach (InputDevice inputDevice in _inputDevices)
        {
            if (inputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                if(inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
                {
                    Debug.Log("Left Trigger Pressed");
                }
                // Left hand, grip button
                ProcessInputDeviceButton(inputDevice, InputHelpers.Button.Trigger, ref _leftTriggerDown,
                () => // On Button Down
            {
                    Debug.Log("Left hand trigger down");
                // Your functionality
            },
                () => // On Button Up
            {
                    Debug.Log("Left hand trigger up");
                });
                // Repeat ProcessInputDeviceButton for other buttons
            }
            // Repeat for right hand
            if (inputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                
                // Left hand, grip button
                ProcessInputDeviceButton(inputDevice, InputHelpers.Button.Trigger, ref _rightTriggerDown,
                () => // On Button Down
                {
                    Debug.Log("Right hand trigger down");
                    // Your functionality
                },
                () => // On Button Up
                {
                    Debug.Log("Right hand trigger up");
                });
                // Repeat ProcessInputDeviceButton for other buttons
            }
        }*/

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        Timer += Time.deltaTime;

        if (Timer > .2f)
        {
            PosInfo = new FileStream(path_Pos, FileMode.Append, FileAccess.Write);
            PosWriter = new StreamWriter(PosInfo, System.Text.Encoding.Unicode);
            PosWriter.WriteLine(GetDetailPos());
            PosWriter.Close();

            Timer = 0;
        }
    }

    string GetDetailPos()
    {
        string Head_x = HEAD_Controller.localPosition.x.ToString();
        string Head_y = HEAD_Controller.localPosition.y.ToString();
        string Head_z = HEAD_Controller.localPosition.z.ToString();

        string Head_angle_x = HEAD_Controller.localRotation.eulerAngles.x.ToString();
        string Head_angle_y = HEAD_Controller.localRotation.eulerAngles.y.ToString();
        string Head_angle_z = HEAD_Controller.localRotation.eulerAngles.z.ToString();
       

        string Hand_Lx = Hand_L_Controller.localPosition.x.ToString();
        string Hand_Ly = Hand_L_Controller.localPosition.y.ToString();
        string Hand_Lz = Hand_L_Controller.localPosition.z.ToString();

        string Hand_Rx = Hand_R_Controller.localPosition.x.ToString();
        string Hand_Ry = Hand_R_Controller.localPosition.y.ToString();
        string Hand_Rz = Hand_R_Controller.localPosition.z.ToString();

        return "HEAD (" + Head_x + ", " + Head_y + ", " + Head_z + ")     HEAD_ANGLE (" + Head_angle_x + ", " + Head_angle_y + ", " + Head_angle_z + ")     Hand_L (" + Hand_Lx + ", " + Hand_Ly + ", " + Hand_Lz + ")     Hand_R (" + Hand_Rx + ", " + Hand_Ry + ", " + Hand_Rz + ")";
    }

    /*private void TriggerLeftPressed(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, bool active)
    {
        Click_L += 1;
        if (active)
        {
            Debug.Log("Click_L" + Click_L.ToString());
        }

        *//*if (leftController.gripButton.isPressed)
        {
            Click_L += 1;
        }*/

        /*UnityEngine.XR.InputDevice device = UnityEngine.XR.InputDevice;

        bool triggerValue;
        if(device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {

        }*//*

        SaveLeft = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");

        LeftInfo = new FileStream(path_Hand_L, FileMode.Append, FileAccess.Write);
        LeftWriter = new StreamWriter(LeftInfo, System.Text.Encoding.Unicode);
        LeftWriter.WriteLine(SaveLeft);
        LeftWriter.WriteLine("Clicks: "+Click_L);
        LeftWriter.Close();
    }

    private void TriggerRightPressed(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, bool active)
    {
        Click_R += 1;
        if (active)
        {
            Debug.Log("Click_R" + Click_R.ToString());
        }
       

        SaveRight = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");

        RightInfo = new FileStream(path_Hand_R, FileMode.Append, FileAccess.Write);
        RightWriter = new StreamWriter(RightInfo, System.Text.Encoding.Unicode);
        RightWriter.WriteLine(SaveRight);
        RightWriter.WriteLine("Clicks: " + Click_R);
        RightWriter.Close();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void ProcessInputDeviceButton(InputDevice inputDevice, InputHelpers.Button button, ref bool _wasPressedDownPreviousFrame, Action onButtonDown = null, Action onButtonUp = null, Action onButtonHeld = null)
    {
        
        if (inputDevice.IsPressed(button, out bool isPressed) && isPressed)
        {
            if (!_wasPressedDownPreviousFrame) // // this is button down
            {
                onButtonDown?.Invoke();
            }

            _wasPressedDownPreviousFrame = true;
            onButtonHeld?.Invoke();
        }
        else
        {
            if (_wasPressedDownPreviousFrame) // this is button up
            {
                onButtonUp?.Invoke();
            }

            _wasPressedDownPreviousFrame = false;
        }
    }*/
}
