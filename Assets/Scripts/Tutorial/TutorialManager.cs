﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using HutongGames.PlayMaker;
using UserData;
using TMPro;


namespace BNG.UserData {
    public class TutorialManager : MonoBehaviour
    {
        UserInfo dataManager;
        private float rTriggerValue;
        private float ltriggerValue;
        private float rGripValue;
        private float lGripValue;
        private InputBridge Controller;
        public GameObject XRRig;
        public PlayMakerFSM Playmaker;
        public GameObject Grabber;
        public GameObject Grabbed;
        public GameObject Note;
        public bool FSMCheck;
        public bool FSMCheck2;
        public GameObject Ghost;
        Outlinable[] childGrabbed;
        Outlinable prechildGrabbed;
        public GameObject heldGrabbable;
        public int trigInt;
        bool check = false;
        private string gradeLH;
        public int SceneIndex;



        // Start is called before the first frame update
        void Start()
        {
            trigInt = 0;
            //GetGrade();
      

        }

        // Update is called once per frame
        void Update()
        {
            childGrabbed = Grabber.GetComponentsInChildren<Outlinable>();

            if (Grabber.GetComponent<Grabber>().HeldGrabbable != null)
            { heldGrabbable = Grabber.GetComponent<Grabber>().HeldGrabbable.transform.gameObject; }



            Controller = XRRig.GetComponent<InputBridge>();
            rTriggerValue = Controller.RightTrigger;
            ltriggerValue = Controller.LeftTrigger;
            lGripValue = Controller.LeftGrip;
            rGripValue = Controller.RightGrip;



        }

        private void LateUpdate()
        {
            if (FSMCheck)
            {
                

                //TriggerDown Check
                if (rTriggerValue > 0.7f && ltriggerValue > 0.7f)
                {

                    Debug.Log("true");
                    //if (check)
                    // {
                    //trigInt += 1;
                    //if (trigInt >= 5)
                    // {
                    Playmaker.FsmVariables.GetFsmBool("triggerDown").Value = true;

                    //  }

                    //   }
                    //   check = false;

                }
                if (lGripValue > 0.7f && rGripValue > 0.7f)
                {
                    Debug.Log("done");
                    string text = "그 버튼이 아니야\n<color=#2e86de>(O _ O)!";
                    ghostSpeak(text);
                   
                }
              

            }

            if (FSMCheck2)
            {
                if (ltriggerValue >= 0.5)
                {
                    Note.SetActive(true);
                    Playmaker.FsmVariables.GetFsmBool("lTriggerPressed").Value = true;


                }
                if (ltriggerValue <= 0.9)
                {
                    Note.SetActive(false);


                }
            }



        }

        public void ghostSpeak(string text)
        {
            int index = 0;
            StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak(text,index ));

        }


        public void GetGrade()
        {
            GameObject JasonManager = GameObject.Find("DataManager");
            dataManager = JasonManager.GetComponent<UserInfo>();
            gradeLH = dataManager.Grade;
            if(gradeLH == "L")
            {
                SceneIndex = 1;


            }
            if(gradeLH == "H")
            {
                SceneIndex = 5;
            }




        }

       
    }

     
    






}
