using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UserData;

namespace BNG
{
    public class CollectData : MonoBehaviour
    {
        // Add Objects in Inspector
        [Header("Objects in Game")]
        [SerializeField] [Tooltip("XR Rig or XR Rig Advanced goes here")] Transform XRRig;
        [SerializeField] [Tooltip("CenterEyeAnchor goes here")] Transform Camera;
        [SerializeField] [Tooltip("LeftControllerAnchor goes here")] Transform LHand;
        [SerializeField] [Tooltip("RightControllerAnchor goes here")] Transform RHand;
        
        private Stats database; // All data is stored in this object
        private InputBridge _inputBridge; // XR Rig Input Bridge (C# Script)

        // Fields needed for Trigger Input (Do Not Remove)
        float RTriggerState = 0;
        float LTriggerState = 0;

        // For Checking in Inspector 
        [Header("Controller Button Debug Panel")]
        public int _LTriggerClicks;
        public float _LTrigger;
        public int _RTriggerClicks;
        public float _RTrigger;
        public int _LGripClicks;
        public float _LGrip;
        public int _RGripClicks;
        public float _RGrip;
        public int _AClicks;
        public int _BClicks;
        public int _XClicks;
        public int _YClicks;

        // File Writing
        FileStream InputDataInfo;
        StreamWriter InputDataWriter;

        string FileName;
        string FolderName;
        string FilePath_Root;
        string FilePath_Folder;

        // Data per Frame
        List<string> dataPerFrame = new List<string>();
        List<string> plotPerFrame = new List<string>();

        string InputSavePath;
        string DeviceSavePath;

        float Timer = 0;
        float plotTimer = 0;
        FileStream DeviceDataInfo;
        StreamWriter DeviceDataWriter;

        // Object Class for storing data
        class Stats
        {
            // Head Transform
            public Vector3 HeadPosition;    // 3D Coordinates (Contains x, y, z)
            public Vector3 HeadAngle;   // Euler Angles (Contains x, y, z)
            // Controller Transform
            public Vector3 LHandPosition;
            public Vector3 RHandPosition;
            public string controllerTransform;

            // Controller Input
            public int LTriggerClicks = 0;
            public int RTriggerClicks = 0;
            public int LGripClicks = 0;
            public int RGripClicks = 0;
            public int AClicks = 0;
            public int BClicks = 0;
            public int XClicks = 0;
            public int YClicks = 0;
            public string controllerInput;

            // Final Output
            public string output;
        }

        // Start is called before the first frame update
        void Start()
        {
            database = new Stats();

            FileName = SceneManager.GetActiveScene().buildIndex.ToString();                                                                                      // SceneManager.GetActiveScene().buildIndex.ToString();
#if UNITY_EDITOR
            FileName = EditorSceneManager.GetActiveScene().buildIndex.ToString();
#endif

            // 아래 코드를 막은 이유는 DataManager의 함수와 중복되는 내용
            //FolderName = "NAME" + DateTime.Now.ToString("yyyyMMddHHdd");                                          // UserData.DataManager.GetInstance().userInfo.Name + "_" + UserData.DataManager.GetInstance().userInfo.Gender;
            //FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";       //기본 날짜 묶음 C:\Users\uk308\AppData\LocalLow\HippoTnC\Strengthen_Concentration_VR
            //FilePath_Folder = FilePath_Root + FolderName + "/";

            //if (!Directory.Exists(FilePath_Root))
            //{
            //    Directory.CreateDirectory(FilePath_Root);
            //}

            //if (!Directory.Exists(FilePath_Folder))
            //{
            //    Directory.CreateDirectory(FilePath_Folder);
            //}
            
            InputSavePath = DataManager.GetInstance().FilePath_Folder + FileName + "_Plot.txt";
            DeviceSavePath = DataManager.GetInstance().FilePath_Folder + FileName + "_Behavior.txt";
        }

        void Update()
        {
            Timer += Time.deltaTime;
            plotTimer += Time.deltaTime;

            if (Timer > .05f)
            {
                SaveDeviceData();
                Plot(plotTimer);
                Timer = 0;
            }
            ShowDataOnInspector();
        }

        public void SaveBehaviorData()     //<<< ------------------------------- 각자 종료할때 호출해서 저장
        {
            database.controllerInput = "Left Trigger Clicks: " + database.LTriggerClicks.ToString() + "\nRight Trigger Clicks: " + database.RTriggerClicks.ToString()
                + "\nLeft Grip Clicks: " + database.LGripClicks.ToString() + "\nRight Grip Clicks: " + database.RGripClicks.ToString()
                + "\nA Button Pressed: " + database.AClicks.ToString() + "\nB Button Pressed: " + database.BClicks.ToString()
                + "\nX Button Pressed: " + database.XClicks.ToString() + "\nY Button Pressed: " + database.YClicks.ToString();

            database.output += database.controllerInput;
            dataPerFrame.Add(database.controllerInput);

            DeviceDataInfo = new FileStream(DeviceSavePath, FileMode.Append, FileAccess.Write);
            DeviceDataWriter = new StreamWriter(DeviceDataInfo, System.Text.Encoding.Unicode);
            foreach (string data in dataPerFrame)
            {
                DeviceDataWriter.WriteLine(data);
            }
            /*DeviceDataWriter.WriteLine(database.output);*/
            DeviceDataWriter.Close();

            InputDataInfo = new FileStream(InputSavePath, FileMode.Append, FileAccess.Write);
            InputDataWriter = new StreamWriter(InputDataInfo, System.Text.Encoding.Unicode);
            InputDataWriter.WriteLine("Time, LTrigger Value, Time, RTrigger Value, Time, LGrip Value, Time, RGrip Value");
            foreach (string plot in plotPerFrame)
            {
                InputDataWriter.WriteLine(plot);
            }
            InputDataWriter.Close();
            // Delete if unnecessary
            //SaveInputData(database.controllerInput);
        }

        void ShowDataOnInspector()
        {
            _inputBridge = XRRig.GetComponent<InputBridge>();

            // Saves Camera Rig Position & EulerAngles, Left Controller Position and Right Controller Position
            database.HeadPosition = Camera.localPosition;
            database.HeadAngle = Camera.eulerAngles;
            database.LHandPosition = LHand.localPosition;
            database.RHandPosition = RHand.localPosition;

            // Trigger Button
            if (RTriggerState < _inputBridge.RightTrigger && _inputBridge.RightTrigger == 1)
            {
                database.RTriggerClicks++;
            }
            if (LTriggerState < _inputBridge.LeftTrigger && _inputBridge.LeftTrigger == 1)
            {
                database.LTriggerClicks++;
            }
            RTriggerState = _inputBridge.RightTrigger; // Save Right Trigger State in current frame
            LTriggerState = _inputBridge.LeftTrigger; // Save Left Trigger State in current frame
            
            // Grip Button
            if (_inputBridge.RightGripDown)
            {
                database.RGripClicks++;
            }
            if (_inputBridge.LeftGripDown)
            {
                database.LGripClicks++;
            }
            // Right Controller
            if (_inputBridge.AButtonDown)
            {
                database.AClicks++;
            }
            if (_inputBridge.BButtonDown)
            {
                database.BClicks++;
            }
            // Left Controller
            if (_inputBridge.XButtonDown)
            {
                database.XClicks++;
            }
            if (_inputBridge.YButtonDown)
            {
                database.YClicks++;
            }

            // Update Debug Panel per frame
            _LTriggerClicks = database.LTriggerClicks;
            _LTrigger = _inputBridge.LeftTrigger;
            _RTriggerClicks = database.RTriggerClicks;
            _RTrigger = _inputBridge.RightTrigger;
            _LGripClicks = database.LGripClicks;
            _LGrip = _inputBridge.LeftGrip;
            _RGripClicks = database.RGripClicks;
            _RGrip = _inputBridge.RightGrip;
            _AClicks = database.AClicks;
            _BClicks = database.BClicks;
            _XClicks = database.XClicks;
            _YClicks = database.YClicks;

            //////////////////////////////////////////
            //////////////////////////////////////////
            // Create Data String
            // CODE GOES HERE
            //////////////////////////////////////////
            //////////////////////////////////////////
        }

        public void SaveDeviceData()
        {
            dataPerFrame.Add(Buttons() + "\n" + Positions() + "\n");
            
            /*DeviceDataInfo = new FileStream(DeviceSavePath, FileMode.Append, FileAccess.Write);
            DeviceDataWriter = new StreamWriter(DeviceDataInfo, System.Text.Encoding.Unicode);
            DeviceDataWriter.WriteLine(Buttons());
            DeviceDataWriter.WriteLine(Positions());
            DeviceDataWriter.Close();*/
        }

        public void Plot(float time)
        {
            string plot = time.ToString() + ", " + _LTrigger.ToString() + ", " + time.ToString() + ", " + _RTrigger.ToString() + ", " + time.ToString() + ", " + _LGrip.ToString() + ", " + time.ToString() + ", " + _RGrip.ToString();
            plotPerFrame.Add(plot);
        }

        public string Positions()
        {
            return "HEAD POSITION (" + database.HeadPosition.x.ToString() + ", " + database.HeadPosition.y.ToString() + ", " + database.HeadPosition.z.ToString() + 
                " )        HEAD ANGLE (" + database.HeadAngle.x.ToString() + ", " + database.HeadAngle.y.ToString() + ", " + database.HeadAngle.z.ToString() + 
                " )\nLEFT CONTROLLER POSITION (" + database.LHandPosition.x.ToString() + ", " + database.LHandPosition.y.ToString() + ", " + database.LHandPosition.z.ToString() + 
                " )        RIGHT CONTROLLER POSITION (" + database.RHandPosition.x.ToString() + ", " + database.RHandPosition.y.ToString() + ", " + database.RHandPosition.z.ToString() + " )"
                + "\n------------------------------------------------------------------------------------------------------------------------------------------------------------";
        }

        public string Buttons()
        {
            return "A Button: " + (_inputBridge.AButtonDown || _inputBridge.AButton || _inputBridge.AButtonUp ? 1 : 0).ToString() +
                "  B Button: " + (_inputBridge.BButtonDown || _inputBridge.BButton || _inputBridge.BButtonUp ? 1 : 0).ToString() +
                "  X Button: " + (_inputBridge.XButtonDown || _inputBridge.XButton || _inputBridge.XButtonUp ? 1 : 0).ToString() +
                "  Y Button: " + (_inputBridge.YButtonDown || _inputBridge.YButton || _inputBridge.YButtonUp ? 1 : 0).ToString() +
                "\nLeft Trigger: " + _LTrigger.ToString() + "  Right Trigger: " + _RTrigger.ToString() + "  Left Grip: " + _LGrip.ToString() + "  Right Grip: " + _RGrip.ToString() + "\n";
        }

        public void SaveInputData(string myData)
        {
            InputDataInfo = new FileStream(InputSavePath, FileMode.Append, FileAccess.Write);
            InputDataWriter = new StreamWriter(InputDataInfo, System.Text.Encoding.Unicode);
            InputDataWriter.WriteLine(myData);
            InputDataWriter.Close();
        }

        public void AddTimeStamp(string delimiter)
        {
            string _delimiter = delimiter + "\n";
            dataPerFrame.Add(_delimiter);
        }
    }   
}


