using UnityEngine;
using HutongGames.PlayMaker;
using BNG;

public class OnReleaseChecker : MonoBehaviour
{
    Grabber grabber;
    string grabbableName;
    GameObject foundGameObj;

    GameObject objToFind;

    FsmObject fsmObject;
    FsmGameObject fsmG_Obj;

    public PlayMakerFSM GoDuckFsm;


    void Start()
    {
        grabber = GetComponent<Grabber>();
    }
    
    string OnGrabObjInfoCheck()
    {
        Initialize();

        grabbableName = grabber.HeldGrabbable.name;

        if (foundGameObj.tag == "Unnecessary")
        {
            SendEvent("Angry sound");
        }

        return grabbableName;
    }

    void ReleaseObjInfoCheck()
    {
        FindGameObj(grabbableName);

        if(foundGameObj.tag == "Necessary")
        {

        }

        else if (foundGameObj.tag == "Unnecessary")
        {
            SendEvent("Quacking sound");
            foundGameObj.GetComponent<Moveable>().SendMessage("SpeedUp");
        }
    }

    GameObject FindGameObj(string name)
    {
        foundGameObj = GameObject.Find(name);
        return foundGameObj;
    }

    void Initialize()
    {
        grabbableName = null;
        foundGameObj = null;
    }    

    private void SendEvent(string currentEvent)
        {
            //fsmG_Obj = GoDuckFsm.FsmVariables.GetFsmGameObject("grab_g");
            GoDuckFsm.SendEvent(currentEvent);
        }
}
