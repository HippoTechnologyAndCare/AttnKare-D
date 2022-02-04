using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using EPOOutline;

public class PCaseCap_BP : MonoBehaviour
{ 
    //UI용 만들어서 case cap activate 해야함
    public Transform Cap;
    Transform m_tParent;
    Object_BP.STATE m_eState;

    Transform m_tChild;
    Vector3 m_v3Pos = new Vector3(2.2e-05f, -2e-06f, 0.000332f);
    Vector3 m_v3Rot = new Vector3(-180, -180, -180);
    Vector3 m_v3Scale = new Vector3(1.050626f, 1.005712f, 1);

    // Start is called before the first frame update
    void Start()
    {
        m_tParent = transform.parent;
        Cap.GetComponentInChildren<RingHelper>().enabled = Cap.GetComponent<Rigidbody>().useGravity
                   = Cap.GetComponent<BoxCollider>().enabled = Cap.GetComponent<Outlinable>().enabled = true;
        Cap.GetComponentInChildren<Animator>().gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_eState)
        {
            case Object_BP.STATE.ENTER: Enter(); break;
            case Object_BP.STATE.EXIT: break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == Cap) { m_eState = Object_BP.STATE.ENTER; }
        //if object is grabbable store it
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == Cap) { m_eState = Object_BP.STATE.EXIT; }
        //if exit collider without releasing, null m_Col
    }

    void Enter()
    {
        if (Object_BP.bGrabbed) return; //wait until trigger is released
        if (!Object_BP.bGrabbed) { m_eState = Object_BP.STATE.EXIT; }
    }

    void CheckCap()
    {
        transform.GetComponent<Collider>().enabled = false;
        m_tChild = Cap.Find("Pencilcase_cover");
        m_tChild.localPosition = m_v3Pos;
        m_tChild.localEulerAngles = m_v3Rot;
        m_tChild.localScale = m_v3Scale;
        Destroy(Cap); 

    }
}
