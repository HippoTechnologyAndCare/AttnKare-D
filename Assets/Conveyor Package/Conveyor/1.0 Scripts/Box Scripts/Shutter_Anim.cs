using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxState
{
    Open,
    Closed,
    Plain,
    Toy
}
public class Shutter_Anim : MonoBehaviour
{
    // Start is called before the first frame update
    Animation Shutter;
    GameObject m_collider = null;
    public BoxState boxType;
    string boxName;
    bool check=true;
    private void Start()
    {
        switch (boxType)
        {
            case BoxState.Open: boxName = "Main Box(Clone)"; break;
            case BoxState.Closed: boxName = "Closed Box(Clone)"; break;
            case BoxState.Plain: boxName = "Box(Clone)"; break;
        }
        Shutter = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == boxName) { Shutter.Play("Shutter_Open"); m_collider = other.gameObject; }
        if(boxType == BoxState.Toy&& check) { Shutter.Play("Shutter_Open"); check = false; }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == m_collider) { Shutter.Play("Shutter_Closed"); m_collider = null; }


    }
}
