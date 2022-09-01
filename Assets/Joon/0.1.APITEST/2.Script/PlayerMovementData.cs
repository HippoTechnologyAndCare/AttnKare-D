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

namespace BNG{
    public class PlayerMovementData : MonoBehaviour
    {
    
/// <summary>
/// 시작과 끝부분마다 GetComponent<BNG.CollectData>().AddTimeStamp("delimiter name");를 호출하면 됩니다
/// </summary>
 

        // Add Objects in Inspector
        [Header("Objects in Game")]
        [SerializeField] [Tooltip("XR Rig or XR Rig Advanced goes here")] Transform XRRig;
        [SerializeField] [Tooltip("CenterEyeAnchor goes here")] Transform Camera;
        [SerializeField] [Tooltip("LeftControllerAnchor goes here")] Transform LHand;
        [SerializeField] [Tooltip("RightControllerAnchor goes here")] Transform RHand;
        public Vector3 LEFTHAND;

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

        public Vector3 currentVelocity;

        // File Writing
        FileStream InputDataInfo;
        StreamWriter InputDataWriter;

        string FileName;

        // Data per Frame
       [SerializeField] List<List<object>> dataPerFrame = new List<List<object>>();
        List<string> plotPerFrame = new List<string>();

        // Velocity
        Vector3 prevPosHead;
        Vector3 curPosHead;
        Vector3 prevPosRHand;
        Vector3 curPosRHand;
        Vector3 prevPosLHand;
        Vector3 curPosLHand;

        float velocityXHead;
        float velocityYHead;
        float velocityZHead;
        float velocityXRHand;
        float velocityYRHand;
        float velocityZRHand;
        float velocityXLHand;
        float velocityYLHand;
        float velocityZLHand;

        // Path
        string InputSavePath;
        string DeviceSavePath;

        float Timer = 0;
        FileStream DeviceDataInfo;
        StreamWriter DeviceDataWriter;
        int frame = 0;
        public float dt; // Time Interval

        // Object Class for storing data
        class Stats
        {
            // Head Transform
            public Vector3 HeadPosition;    // 3D Coordinates (Contains x, y, z)
            public Vector3 HeadAngle;   // Euler Angles (Contains x, y, z

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
            public List<object> controllerInput = new List<object> ();

            // Final Output
            public string output;
        }

        // Start is called before the first frame update
        void Start()
        {
            database = new Stats();

            FileName = SceneManager.GetActiveScene().buildIndex.ToString();
#if UNITY_EDITOR
            FileName = EditorSceneManager.GetActiveScene().buildIndex.ToString();
#endif

            // 아래 코드를 막은 이유는 DataManager의 함수와 중복되는 내용
            // FolderName = "NAME" + DateTime.Now.ToString("yyyyMMddHHdd");                                          // UserData.DataManager.GetInstance().userInfo.Name + "_" + UserData.DataManager.GetInstance().userInfo.Gender;
            // FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";       //기본 날짜 묶음 C:\Users\uk308\AppData\LocalLow\HippoTnC\Strengthen_Concentration_VR
            // FilePath_Folder = FilePath_Root + FolderName + "/";

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

            ShowDataOnInspector();
            //dataPerFrame.Add("Time, A, B, X, Y, LTr, RTr, LGr, RGr, HPosX, HPosY, HPosZ, HAngX, HAngY, HAngZ, LHPosX, LHPosY, LHPosZ, RHPosX, RHPosY, RHPosZ");*/
            prevPosHead = database.HeadPosition;
            prevPosLHand = database.LHandPosition;
            prevPosRHand = database.RHandPosition;
        }

        void FixedUpdate()
        {
            Timer += Time.deltaTime;
            dt += Time.deltaTime;
            frame++;
            LEFTHAND = database.LHandPosition;
            if (frame > 3)
            {
                SaveDeviceData();
                Plot(Timer);
                frame = 0;
                dt = 0;
            }

            ShowDataOnInspector();
        }

        // 30
        // Time, A, B, X, Y, LTr, RTr, LGr, RGr, HPosX, HPosY, HPosZ, HAngX, HAngY, HAngZ, LHPosX, LHPosY, LHPosZ, RHPosX, RHPosY, RHPosZ, HeadVelX, HeadVelY, HeadVelZ, LHandVelX, LHandVelY, LHandVelZ, RHandVelX, RHandVelY, RHandVelZ
        /*public void SaveBehaviorData()     //<<< ------------------------------- 각자 종료할때 호출해서 저장
        {
            database.controllerInput = "Left Trigger Clicks: " + database.LTriggerClicks.ToString() + "\nRight Trigger Clicks: " + database.RTriggerClicks.ToString()
                + "\nLeft Grip Clicks: " + database.LGripClicks.ToString() + "\nRight Grip Clicks: " + database.RGripClicks.ToString()
                + "\nA Button Pressed: " + database.AClicks.ToString() + "\nB Button Pressed: " + database.BClicks.ToString()
                + "\nX Button Pressed: " + database.XClicks.ToString() + "\nY Button Pressed: " + database.YClicks.ToString();
          
            // Add Number of Clicks to Data List
            dataPerFrame.Add(database.controllerInput);

            DeviceDataInfo = new FileStream(DeviceSavePath, FileMode.Append, FileAccess.Write);
            DeviceDataWriter = new StreamWriter(DeviceDataInfo, System.Text.Encoding.Unicode);
            
            foreach (string data in dataPerFrame)
            {
                DeviceDataWriter.WriteLine(data);
            }
            DeviceDataWriter.Close();
            ///여기서 바로 보내기
            InputDataInfo = new FileStream(InputSavePath, FileMode.Append, FileAccess.Write);
            InputDataWriter = new StreamWriter(InputDataInfo, System.Text.Encoding.Unicode);
            InputDataWriter.WriteLine("Time, LTrigger Value, Time, RTrigger Value, Time, LGrip Value, Time, RGrip Value");
            foreach (string plot in plotPerFrame)
            {
                InputDataWriter.WriteLine(plot);
            }
            InputDataWriter.Close();

            // Delete if unnecessary
            // SaveInputData(database.controllerInput);
        }
*/
        public List<List<object>> GetBehaviorData()
        {
            Debug.Log("GET DATA");
            /*
            database.controllerInput = "Left Trigger Clicks: " + database.LTriggerClicks.ToString() + "Right Trigger Clicks: " + database.RTriggerClicks.ToString()
                + "Left Grip Clicks: " + database.LGripClicks.ToString() + "Right Grip Clicks: " + database.RGripClicks.ToString()
                + "A Button Pressed: " + database.AClicks.ToString() + "B Button Pressed: " + database.BClicks.ToString()
                + "X Button Pressed: " + database.XClicks.ToString() + "Y Button Pressed: " + database.YClicks.ToString();
             */
          /*  database.controllerInput.Add(database.LTriggerClicks); database.controllerInput.Add(database.RTriggerClicks);
            database.controllerInput.Add(database.LGripClicks); database.controllerInput.Add(database.RGripClicks);
            database.controllerInput.Add(database.AClicks); database.controllerInput.Add(database.BClicks);
            database.controllerInput.Add(database.XClicks); database.controllerInput.Add(database.YClicks);*/ //임시로 삭제
           
            // Add Number of Clicks to Data List
            dataPerFrame.Add(database.controllerInput);
            Debug.Log(dataPerFrame[2]);
            string datalist = "";
            for (int i = 0; i < dataPerFrame.Count; i++)
            {
                datalist += dataPerFrame[i];
                datalist += "/";
            }
            return dataPerFrame;
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
        }

        public void SaveDeviceData()
        {
            //                    Time            Controller Buttons    Device Position      Device Velocity
          //  dataPerFrame.Add(Timer.ToString() + ", " + Buttons() + ", " + Positions() + ", " + Velocity());
            List<object> dataPerLine = new List<object>();
            dataPerLine.Add(Timer);
            dataPerLine.AddRange(Buttons());
            dataPerLine.AddRange(Positions());
           dataPerLine.AddRange(Velocity());
            dataPerFrame.Add(dataPerLine);
        }

        public void Plot(float time)
        {
            string plot = time.ToString() + ", " + _LTrigger.ToString() + ", " + time.ToString() + ", " + _RTrigger.ToString() + ", " + time.ToString() + ", " + _LGrip.ToString() + ", " + time.ToString() + ", " + _RGrip.ToString();
            plotPerFrame.Add(plot);
        }

        public List<object> Velocity()
        {
            // Return Value
            string result = "";
            List<object> listResult = new List<object>();

            // Update Current Position
            curPosHead = database.HeadPosition;
            curPosLHand = database.LHandPosition;
            curPosRHand = database.RHandPosition;

            // Get xyz components of velocity vector
            // Head
            velocityXHead = (curPosHead.x - prevPosHead.x) / dt;
            velocityYHead = (curPosHead.y - prevPosHead.y) / dt;
            velocityZHead = (curPosHead.z - prevPosHead.z) / dt;
            // Left Hand
            velocityXLHand = (curPosLHand.x - prevPosLHand.x) / dt;
            velocityYLHand = (curPosLHand.y - prevPosLHand.y) / dt;
            velocityZLHand = (curPosLHand.z - prevPosLHand.z) / dt;
            // Right Hand
            velocityXRHand = (curPosRHand.x - prevPosRHand.x) / dt;
            velocityYRHand = (curPosRHand.y - prevPosRHand.y) / dt;
            velocityZRHand = (curPosRHand.z - prevPosRHand.z) / dt;

            // Save Current State
            prevPosHead = curPosHead;
            prevPosLHand = curPosLHand;
            prevPosRHand = curPosRHand;

            // Save to Return Value
            listResult.Add(velocityXHead); listResult.Add(velocityYHead); listResult.Add(velocityZHead); 
            listResult.Add(velocityXLHand); listResult.Add(velocityYLHand); listResult.Add(velocityZLHand);
            listResult.Add(velocityXRHand); listResult.Add(velocityYRHand); listResult.Add(velocityZRHand);
            //  result += velocityXHead.ToString() + ", " + velocityYHead.ToString() + ", " + velocityZHead.ToString() + ", ";
            //  result += velocityXLHand.ToString() + ", " + velocityYLHand.ToString() + ", " + velocityZLHand.ToString() + ", ";
            //  result += velocityXRHand.ToString() + ", " + velocityYRHand.ToString() + ", " + velocityZRHand.ToString();

            // Return Velocity
            return listResult;
            //return result;
        }

        public List<object> Positions()
        {
            List<object> listResult = new List<object>();
            listResult.Add(database.HeadPosition.x); listResult.Add(database.HeadPosition.y); listResult.Add(database.HeadPosition.z);
            listResult.Add(database.HeadAngle.x); listResult.Add(database.HeadAngle.y); listResult.Add(database.HeadAngle.z);
            listResult.Add(database.LHandPosition.x); listResult.Add(database.LHandPosition.y); listResult.Add(database.LHandPosition.z);
            listResult.Add(database.RHandPosition.x); listResult.Add(database.RHandPosition.y); listResult.Add(database.RHandPosition.z);
          return listResult;

            /*  return database.HeadPosition.x.ToString() + ", " + database.HeadPosition.y.ToString() + ", " + database.HeadPosition.z.ToString() +
                   ", " + database.HeadAngle.x.ToString() + ", " + database.HeadAngle.y.ToString() + ", " + database.HeadAngle.z.ToString() +
                   ", " + database.LHandPosition.x.ToString() + ", " + database.LHandPosition.y.ToString() + ", " + database.LHandPosition.z.ToString() +
                   ", " + database.RHandPosition.x.ToString() + ", " + database.RHandPosition.y.ToString() + ", " + database.RHandPosition.z.ToString();

             */
        }

        public List<object> Buttons()
        {
            List<object> listResult = new List<object>();
            listResult.Add(_inputBridge.AButtonDown || _inputBridge.AButton || _inputBridge.AButtonUp ? 1 : 0); 
            listResult.Add(_inputBridge.BButtonDown || _inputBridge.BButton || _inputBridge.BButtonUp ? 1 : 0);
            listResult.Add(_inputBridge.XButtonDown || _inputBridge.XButton || _inputBridge.XButtonUp ? 1 : 0);
            listResult.Add(_inputBridge.YButtonDown || _inputBridge.YButton || _inputBridge.YButtonUp ? 1 : 0);
            listResult.Add(_LTrigger); listResult.Add(_RTrigger); listResult.Add(_LGrip); listResult.Add(_RGrip);
            return listResult;
            /*
            return (_inputBridge.AButtonDown || _inputBridge.AButton || _inputBridge.AButtonUp ? 1 : 0).ToString() +
                ", " + (_inputBridge.BButtonDown || _inputBridge.BButton || _inputBridge.BButtonUp ? 1 : 0).ToString() +
                ", " + (_inputBridge.XButtonDown || _inputBridge.XButton || _inputBridge.XButtonUp ? 1 : 0).ToString() +
                ", " + (_inputBridge.YButtonDown || _inputBridge.YButton || _inputBridge.YButtonUp ? 1 : 0).ToString() +
                ", " + _LTrigger.ToString() + ", " + _RTrigger.ToString() + ", " + _LGrip.ToString() + ", " + _RGrip.ToString();
            */
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
            string _delimiter = delimiter;
            List<object> addDeli = new List<object>();
            addDeli.Add(_delimiter);
            dataPerFrame.Add(addDeli);
          //  dataPerFrame.Add(_delimiter);
        }
    }
}

