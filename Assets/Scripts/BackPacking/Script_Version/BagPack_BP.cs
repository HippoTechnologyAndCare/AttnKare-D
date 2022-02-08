using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPack_BP : MonoBehaviour
{

    Object_BP.STATE m_eState;
    // Start is called before the first frame updateq
    Transform m_tCol;
    public Transform m_tChild;
    public int unnecessary;
    public int necessary;
    Object_BP.KIND_BP m_eKind;
    Object_BP.BP_INFO m_Bpinfo;
    int m_nPosIndex = 0;
    Transform m_tParent;
    Transform m_tPrevParent;
    GrabObj_BP m_GOBJ;
    List<Transform> m_tTxt = new List<Transform>();
    Transform m_tGlue;
    Transform m_tPencilCase;
    void Start()
    {
        m_eState = Object_BP.STATE.EXIT;
        m_tTxt.Add(transform.Find("TextBook1").transform);
        m_tTxt.Add(transform.Find("TextBook2").transform);
        m_tTxt.Add(transform.Find("TextBook3").transform);
        m_tTxt.Add(transform.Find("TextBook4").transform);

        m_tPencilCase = transform.Find("PencilCase_Final").transform;
        m_tGlue = transform.Find("glue").transform;
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
        if (collision.transform.GetComponent<GrabObj_BP>() & m_tCol == null) { m_tCol = collision.transform; m_eState = Object_BP.STATE.ENTER; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == m_tCol) { m_eState = Object_BP.STATE.EXIT; m_tCol = null; }
    }

    void Enter()
    {
        if(Object_BP.bGrabbed) return; //wait until trigger is released
        if (!Object_BP.bGrabbed) { CheckObj(m_tCol); m_eState = Object_BP.STATE.EXIT; }
    }
    void CheckObj(Transform obj)
    {
        switch (obj.tag)
        {
            case "Necessary": CheckCorrect(obj); break;
            case "Necessary_Pencil": necessary++; obj.GetComponent<GrabObj_BP>().ResetPosition(); break;
            case "Unnecessary": unnecessary++; obj.GetComponent<GrabObj_BP>().ResetPosition(); break;
        }
        m_tCol = null;
    }
    void CheckCorrect(Transform obj)
    {
        m_tParent = obj;
        m_GOBJ = m_tParent.GetComponent<GrabObj_BP>();
        m_tChild = m_tParent.GetComponentInChildren<MeshRenderer>().transform;
        switch (m_GOBJ.eObj)
        {
            case Object_BP.OBJ_BP.TXTBOOK: SetTextbook(); break;
            case Object_BP.OBJ_BP.PCASE: SetPosition(m_tPencilCase); break;
            case Object_BP.OBJ_BP.GLUE: SetPosition(m_tGlue); break;
            default: Debug.Log(m_tChild.name); return;
        }
    }

    void SetPosition(Transform m_tTarget)
    {
        m_tPrevParent = m_tChild.parent;
        m_tChild.SetParent(this.transform);
        m_tChild.localPosition = m_tTarget.localPosition;
        m_tChild.localEulerAngles = m_tTarget.localEulerAngles;
        m_tChild.localScale = m_tTarget.localScale;
        Destroy(m_tTarget.gameObject);
        Destroy(m_tPrevParent.gameObject);
        ResetVariable();

    }
    void SetTextbook()
    {
        m_eKind = m_GOBJ.eKind;
        m_Bpinfo = Object_BP.BP2DB[(int)m_eKind];
        Debug.Log(m_Bpinfo.bCorrect.ToString());
        Debug.Log(m_Bpinfo.eKind.ToString());
        if (m_Bpinfo.bCorrect)
        {
            SetPosition(m_tTxt[m_nPosIndex]);
            m_nPosIndex++;
        }
        else m_GOBJ.ResetPosition();
    }

    void ResetVariable()
    {
        m_tParent = m_tChild = m_tCol = null;
        m_GOBJ = null;
    }
}
