using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPack_BP : MonoBehaviour
{

    Object_BP.STATE m_eState;
    // Start is called before the first frame updateq
    Transform m_tCol;
    public Transform m_tChild;
    UI_BP Hud;
    public int unnecessary;
    public int necessary;
    public bool bStage2 = false;
    int m_nPosIndex = 0;
    Transform m_tParent;
    Transform m_tPrevParent;
    GrabObj_BP m_GOBJ;
    List<Transform> m_tTxt = new List<Transform>();
    Transform m_tGlue;
    Transform m_tPencilCase;
    int m_nAllDone = 0;
    int a;

    void Start()
    {
        Hud = GameObject.Find("UI").GetComponent<UI_BP>();
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
        if (!bStage2) // if stage 1, cannot put obj in bag
        {
            ResetVariable(obj, 4); //add warning
            return;
        }
        switch (obj.tag)
        {
            case "Necessary": CheckCorrect(obj); break;
            case "Necessary_Pencil": necessary++; ResetVariable(obj, 1); break;
            case "Unnecessary": unnecessary++; ResetVariable(obj, 3); break;
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
        m_nAllDone++;
        m_tPrevParent = m_tChild.parent;
        m_tChild.SetParent(this.transform);
        m_tChild.localPosition = m_tTarget.localPosition;
        m_tChild.localEulerAngles = m_tTarget.localEulerAngles;
        m_tChild.localScale = m_tTarget.localScale;
        Destroy(m_tTarget.gameObject);
        Destroy(m_tPrevParent.gameObject);
        if (m_nAllDone >= 6) AllDone();

    }
    void SetTextbook()
    {
        string name = m_GOBJ.name;
        if (name == "txtB_Korean" || m_GOBJ.name == "txtB_Science" || m_GOBJ.name == "txtB_Art" || m_GOBJ.name == "txtB_English")
        {
            SetPosition(m_tTxt[m_nPosIndex]);
            m_nPosIndex++;
        }
        else ResetVariable(m_tParent,2);
    }

    void ResetVariable(Transform obj, int index)
    {
        switch (index)
        {
            case 1: StartCoroutine(Hud.WrongBag(index)); break; //pencil
            case 2: StartCoroutine(Hud.WrongBag(index)); break; //book
            case 3: StartCoroutine(Hud.WrongBag(index)); break; //memo
            case 4: StartCoroutine(Hud.WrongBag(index)); break;
        }
        obj.GetComponent<GrabObj_BP>().ResetPosition();
        m_tParent = m_tChild = m_tCol = null;
        m_GOBJ = null;
    }

    void AllDone()
    {
        Debug.Log("AllDone");
    }
    //책 잘못 넣으면 시간표 확인
    //물건 잘못 넣으면 알림장 확인
    //연필을 넣으면 아니라고 하기
}
