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

    int m_nPosIndex = 0;
    Transform m_tParent;
    GrabObj_BP m_GOBJ;
    Material m_matCol;
    List<Material> m_lMat = new List<Material>();
    Material m_matGlue;
    Material m_matPencilCase;
    void Start()
    {
        m_eState = Object_BP.STATE.EXIT;
        m_lMat.Add(GameObject.Find("TextBook1").GetComponent<Material>());
        m_lMat.Add(GameObject.Find("TextBook2").GetComponent<Material>());
        m_lMat.Add(GameObject.Find("TextBook3").GetComponent<Material>());
        m_lMat.Add(GameObject.Find("TextBook4").GetComponent<Material>());

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
        m_tChild = obj.GetComponentInChildren<MeshRenderer>().transform;
        m_matCol = m_tChild.GetComponent<Material>();
        switch (m_GOBJ.eObj)
        {
            case Object_BP.OBJ_BP.TXTBOOK: SetTextbook(); break;
            case Object_BP.OBJ_BP.PCASE: SetPCase(); break;
            case Object_BP.OBJ_BP.GLUE: SetGlue(); break;
            default: Debug.Log(m_tChild.name); return;
        }
    }
    void SetPCase()
    {
        m_matPencilCase = m_matCol;
    }
    void SetGlue()
    {
        m_matGlue = m_matCol;
    }
    void SetTextbook()
    {
        if (Object_BP.BP2DB[(int)m_GOBJ.eKind].bCorrect)
        {
            m_lMat[m_nPosIndex] = m_matCol;
            m_nPosIndex++;

        }
        else m_GOBJ.ResetPosition();
    }

    void ResetVariable()
    {
        m_tParent = m_tChild = m_tCol = null;
        m_matCol = null;
    }
}
