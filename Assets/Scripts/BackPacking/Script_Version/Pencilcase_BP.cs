using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencilcase_BP : MonoBehaviour
{
    // Start is called before the first frame update
    enum STATE { ENTER, EXIT }
    STATE m_eState = STATE.EXIT;

    Collision m_Col;
    bool check;
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
        if(collision.transform.GetComponent<GrabObj_BP>()) {  m_Col = collision; Debug.Log(m_Col.gameObject.name); m_eState = STATE.ENTER; }
        //if object is grabbable store it
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision == m_Col) { m_eState = STATE.EXIT; m_Col = null; Debug.Log("EXOT");}
        //if exit collider without releasing, null m_Col
    }

    void Enter()
    {
        if (check)
        {
            if (Object_BP.bGrabbed) return;
            if (!Object_BP.bGrabbed) { Debug.Log(m_Col.gameObject.name); CheckObj(m_Col.transform); m_eState = STATE.EXIT; }
        }

    }

    void CheckObj(Transform obj)
    {
        if(!obj.CompareTag("Necessary_Pencil")) { obj.GetComponent<GrabObj_BP>().Reset(); }
    }
    void Pressed()
    {

    }
}
