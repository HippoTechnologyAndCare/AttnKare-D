using UnityEngine;
using BNG;

public class OnReleaseChecker : MonoBehaviour
{
    Grabber grabber;
    string grabbableName;
    GameObject foundGameObj;

    GameObject objToFind;

    void Start()
    {
        grabber = GetComponent<Grabber>();
    }
    
    string OnGrabObjInfoCheck()
    {
        Initialize();

        grabbableName = grabber.HeldGrabbable.name;

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
}
