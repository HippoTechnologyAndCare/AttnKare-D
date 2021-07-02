using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;


namespace BNG {


public class getLTrigger : MonoBehaviour
{

    public float lTrigger;
    public GameObject XRRig;
    public bool ex;
    public GameObject Note;
    public PlayMakerFSM GameFlow;
    public PlayMakerFSM datacheck;
    public bool LtriggerDown;
    public bool set;

        private void Start()
        {
            set = true;
        }





        // Start is called before the first frame update


        // Update is called once per frame
        void Update()
    {
            LtriggerDown = XRRig.GetComponent<InputBridge>().LeftTriggerDown;
            lTrigger = XRRig.GetComponent<InputBridge>().LeftTrigger;
        if(ex == true)
        {GameFlow.FsmVariables.GetFsmBool("bCount").Value = true;}


        if(ex == false)
        {GameFlow.FsmVariables.GetFsmBool("bCount").Value = false;}






        }

        void LateUpdate()
        {

            if (lTrigger >= 0.5)
            {
                Note.SetActive(true);
                GameFlow.FsmVariables.GetFsmBool("bCount").Value = true;
                
                








            }
            if (lTrigger <= 0.9)
            {
                Note.SetActive(false);
                


            }
            
            DataCheck();





        }

        void DataCheck()
        {

            if (LtriggerDown == true)
            {
                if (set)
                {
                    datacheck.Fsm.Event("Note Checking");
                    
                    
                }
                set = false;
            }
            if(LtriggerDown==false)
            {
                set = true;
            }

            



        }






    }

}
