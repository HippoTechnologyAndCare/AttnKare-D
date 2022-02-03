using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencilcase_BP : MonoBehaviour
{
    // Start is called before the first frame update
    enum STATE { ENTER, EXIT }
    STATE m_eState;

    Collision m_Col;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_eState)
        {
            case STATE.ENTER: Enter(); break;
            case STATE.EXIT: break;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10) { m_Col = collision; m_eState = STATE.ENTER; }
        //if object is grabbable store it
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision == m_Col) { m_eState = STATE.EXIT; m_Col = null; }
        //if exit collider without releasing, null m_Col
    }

    void Enter()
    {
        if (Object_BP.bGrabbed) return;
        if (!Object_BP.bGrabbed) { CheckObj(m_Col.transform); m_eState = STATE.EXIT; }
    }

    void CheckObj(Transform obj)
    {
        if(obj.tag != "Necessary_Pencil") { obj.GetComponent<GrabObj_BP>().Reset(); }
    }
    void Pressed()
    {

    }
}
