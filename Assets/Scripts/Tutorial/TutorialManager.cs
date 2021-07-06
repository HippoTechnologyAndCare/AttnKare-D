using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using HutongGames.PlayMaker;


namespace BNG { 
public class TutorialManager : MonoBehaviour
{

        private bool lTrigger_Down;
        private bool rTrigger_Down;
        private float ltriggerValue;
        private InputBridge Controller;
        public GameObject XRRig;
        public PlayMakerFSM Playmaker;
        public GameObject Grabber;
        public GameObject Grabbed;
        public GameObject Note;
        public bool FSMCheck;
        public bool FSMCheck2;
        Outlinable[] childGrabbed;
        Outlinable prechildGrabbed;
        public GameObject heldGrabbable;
        public int trigInt;
        bool check = false;

       

    // Start is called before the first frame update
    void Start()
    {
            trigInt = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
            childGrabbed = Grabber.GetComponentsInChildren<Outlinable>();
            heldGrabbable = Grabber.GetComponent<Grabber>().HeldGrabbable.transform.gameObject;


            
            Controller = XRRig.GetComponent<InputBridge>();
            rTrigger_Down= Controller.RightTriggerDown;
            lTrigger_Down = Controller.LeftTriggerDown;
            ltriggerValue = Controller.LeftTrigger;



    }

        private void LateUpdate()
        {
            if(FSMCheck)
            {
                //TriggerDown Check
                if (lTrigger_Down == true && rTrigger_Down == true)
                {

                    if (check)
                    {
                        //trigInt += 1;
                        //if (trigInt >= 5)
                       // {
                            Playmaker.FsmVariables.GetFsmBool("triggerDown").Value = true;

                      //  }

                    }
                    check = false;

                }
                else if (lTrigger_Down == false || rTrigger_Down == false)
                {
                    check = true;
                }

            }


            //Right Trigger Check (BOX 다섯개 넣어야하는 걸로 수정)
           /* if(childGrabbed.Length > 0)
            {
                Grabbed = childGrabbed[0].gameObject;
                Playmaker.FsmVariables.GetFsmBool("rTriggerPressed").Value = true;
                childGrabbed[0].enabled = false;

            }
           */





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
    }


}
