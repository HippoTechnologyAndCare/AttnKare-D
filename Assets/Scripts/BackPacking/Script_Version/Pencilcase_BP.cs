using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Pencilcase_BP : MonoBehaviour
{
    // Start is called before the first frame update
    Object_BP.STATE m_eState;


    public Transform m_tCol;
    public Transform finalPC;
    Transform m_pencilParent;
    Transform m_tParent;
    public int unnecessary;
    public int necessary;
    UI_BP Hud;
    GameObject Manager;
    int m_nPosIndex = 1;
    int m_nPncilIndex=0;
    int m_nStg1 = 0;
    Vector3[] arr_Pos;
    RingHelper m_ring;
    Grabbable m_grabbable;
    Vector3 m_v3SetRot;
    public Transform m_tChild;
    GrabObj_BP m_GOBJ;
    int a;
    void Start()
    {
        Manager = GameObject.Find("GameFlow_Manager");
        Hud = GameObject.Find("UI").GetComponent<UI_BP>();
        m_eState = Object_BP.STATE.EXIT;
        m_pencilParent = this.transform.parent;
        float m_fY = -0.00029f;
        float m_fZ = 4e-05f;
        arr_Pos = new Vector3[] { new Vector3(-1e-05f, 0.000851f, 4.4e-05f), //���찳 �ڸ�
                                  new Vector3(0.000327f, m_fY, m_fZ), //�Ʒ������ʹ� ����, �� �ڸ��ε� ������ ������� set �ϱ�
                                  new Vector3(0.000182f, m_fY, m_fZ),
                                  new Vector3(0, m_fY, m_fZ),
                                  new Vector3(-0.000151f, m_fY, m_fZ),
                                  new Vector3(-0.000319f, m_fY, m_fZ)
        };
        m_v3SetRot = new Vector3(0, -90, 0);
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
        if(collision.transform.GetComponent<GrabObj_BP>()& m_tCol==null) {  m_tCol = collision.transform; m_eState = Object_BP.STATE.ENTER; }
        //if object is grabbable store it
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform == m_tCol) { m_eState = Object_BP.STATE.EXIT; m_tCol = null;}
        //if exit collider without releasing, null m_Col
    }

    void Enter()
    {
        if (Object_BP.bGrabbed) return; //wait until trigger is released
        if (!Object_BP.bGrabbed) { CheckObj(m_tCol); m_eState = Object_BP.STATE.EXIT; }
    }

    void CheckObj(Transform obj)
    {
        switch (obj.tag)
        {
            case "Necessary_Pencil": CheckCorrect(obj); break;
            case "Necessary": ResetVariables(obj.GetComponent<GrabObj_BP>()); break;
            case "Unnecessary": ResetVariables(obj.GetComponent<GrabObj_BP>()); break;
        }
        m_tCol = null;
    }

    void CheckCorrect(Transform obj)
    {
        m_tParent = obj;
        m_GOBJ= m_tParent.GetComponent<GrabObj_BP>();
        m_tChild = obj.GetComponentInChildren<MeshRenderer>().transform;
        switch (m_GOBJ.eObj)
        {
            case Object_BP.OBJ_BP.PENCIL : SetPencil(); break;
            case Object_BP.OBJ_BP.PEN: SetPen(); break;
            case Object_BP.OBJ_BP.ERASER: SetPosition(true); break; //���찳�� ���� Ȯ���Ұ� ���� �ٷ� ũ�� ��������
            default: Debug.Log(m_tChild.name); return;
        }
    }

   
    void SetPosition(bool bEraser)
    {
        m_tChild.SetParent(m_pencilParent);
        if (bEraser) {
            m_tChild.localPosition = arr_Pos[0];
            m_tChild.localEulerAngles = new Vector3(0, 0, -90);
            m_tChild.localScale = new Vector3(0.7440935f, 0.7357486f, 0.71467f);
        }
        if(!bEraser){
            m_tChild.localPosition = arr_Pos[m_nPosIndex];
            m_nPosIndex++;
            m_tChild.localEulerAngles = m_v3SetRot; }
        m_ring = m_tParent.GetComponentInChildren<RingHelper>(); m_ring.gameObject.SetActive(false);
        m_grabbable = m_tParent.GetComponent<Grabbable>(); m_grabbable.enabled = false;
        Destroy(m_tParent.gameObject);
        m_nStg1++;
        if(m_nStg1 == 6) StartCoroutine(AllDone());//wait for seconds end activate pcap
    }
    void SetPencil()
    {
        if (m_nPncilIndex < 3)
        {
            SetPosition(false);
            m_tChild.localScale = new Vector3(1, 1, 1);
            m_nPncilIndex++;
        }
        else ResetVariables(m_GOBJ);
    }
    void SetPen()
    {
        if(Object_BP.BP1DB[(int)m_GOBJ.eKind].bCorrect)
        {
            SetPosition(false);
            m_tChild.localScale = new Vector3(0.5241489f, 1.009642f, 1.028336f);
        }
        else ResetVariables(m_GOBJ);
    }

    IEnumerator AllDone()
    {
        yield return new WaitForSeconds(1.0f);
        transform.SetParent(finalPC);
        finalPC.gameObject.SetActive(true);
        Manager.GetComponent<Object_BP>().Stage2();
    }

    void ResetVariables(GrabObj_BP obj)
    {
        StartCoroutine(Hud.WrongPencil());
        necessary++;
        obj.ResetPosition();
        m_tParent = m_tChild = m_tCol = null;
        m_GOBJ = null;
    }
   
}
