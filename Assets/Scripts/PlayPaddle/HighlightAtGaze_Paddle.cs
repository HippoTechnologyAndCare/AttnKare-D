﻿// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved
using Tobii.G2OM;
using UnityEngine;

namespace Tobii.XR.Examples
{
    public class HighlightAtGaze_Paddle : MonoBehaviour, IGazeFocusable
    {
        public Transform MainManager; //PaddleManager
        //추후 Object 추가 예정

        public void GazeFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                MainManager.GetComponent<PaddleManager>().DoNotConcentrate = false;
            }
            else
            {
                MainManager.GetComponent<PaddleManager>().DoNotConcentrate = true;
            }
        }
    }
}
