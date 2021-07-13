using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;


public class SceneChange_Circle : MonoBehaviour
{

    public PlayMakerFSM watchManager;
    PlayMakerFSM selfFSM;
    public bool watchBool;
    public bool stayBool;
    bool eventStart;

    // Start is called before the first frame update
    void Start()
    {
        eventStart = false;
        selfFSM = this.gameObject.GetComponent<PlayMakerFSM>();

    }

    // Update is called once per frame
    void Update()
    {
        watchBool = FsmVariables.GlobalVariables.GetFsmBool("b_panelWatch").Value;



    }

    private void LateUpdate()
    {

        if (!watchBool)
        {

            // b_changeScene = false;
            StillMoving();




        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            stayBool = true;
            if (watchBool)
            {
                if(eventStart !=true)
                { StayStill(); }
                
               


            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            StillMoving();
            

        }
    }

    public void StayStill()
    {

        selfFSM.SendEvent("StayStill");
        eventStart = true;



    }

    public void StillMoving()
    {
        selfFSM.SendEvent("StillMoving");
        eventStart = false;


    }
}
