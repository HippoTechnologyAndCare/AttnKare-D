using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public int _RTriggerClicks;
        public int _LGripClicks;
        public int _RGripClicks;
        public int _AClicks;
        public int _BClicks;
        public int _XClicks;
        public int _YClicks;

        // Object Class for storing data
        class Stats
        {
            // Head Transform
            public Vector3 HeadPosition;    // 3D Coordinates (Contains x, y, z)
            public Vector3 HeadAngle;   // Euler Angles (Contains x, y, z)
            // Controller Transform
            public Vector3 LHandPosition;
            public Vector3 RHandPosition;

            // Controller Input
            public int LTriggerClicks = 0;
            public int RTriggerClicks = 0;
            public int LGripClicks = 0;
            public int RGripClicks = 0;
            public int AClicks = 0;
            public int BClicks = 0;
            public int XClicks = 0;
            public int YClicks = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            database = new Stats();  
        }

        // Update is called once per frame
        void Update()
        {
            saveData();
        }

        void saveData()
        {
            _inputBridge = XRRig.GetComponent<InputBridge>();

            // Saves Camera Rig Position & EulerAngles, Left Controller Position and Right Controller Position
            database.HeadPosition = Camera.position;
            database.HeadAngle = Camera.eulerAngles;
            database.LHandPosition = LHand.position;
            database.RHandPosition = RHand.position;

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
            _RTriggerClicks = database.RTriggerClicks;
            _LGripClicks = database.LGripClicks;
            _RGripClicks = database.RGripClicks;
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
    }   
}


