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
    
    // Start is called before the first frame update
    void Start()
    {
        selfFSM = this.gameObject.GetComponent<PlayMakerFSM>();
        
    }

    // Update is called once per frame
    void Update()
    {
        watchBool = FsmVariables.GlobalVariables.GetFsmBool("b_panelWatch").Value;

        
        
    }

    private void LateUpdate()
    {

        if (!watchBool || !stayBool)
        {
            Debug.Log("OUT");
            selfFSM.Fsm.Event("Moved");

        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            stayBool = true;
            if(watchBool)
            {
                Debug.Log("IN");
                selfFSM.Fsm.Event("StayStill");

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            stayBool = false;
            selfFSM.Fsm.Event("Moved");
        }
    }

}
