using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class GrabObj_BP : MonoBehaviour
{
    public Object_BP.OBJ_BP eObj;
    public Object_BP.KIND_BP eKind;
    Transform m_tParent;
    Vector3 m_v3Pos;
    Vector3 m_v3Rot;
    void Start()
    {
        m_tParent = transform.parent;
        m_v3Pos = transform.localPosition;
        m_v3Rot = transform.localEulerAngles;
    }

    public void ResetPosition()
    {
        transform.SetParent(m_tParent);
        transform.localEulerAngles = m_v3Rot;
        transform.localPosition = m_v3Pos;
        Debug.Log("RESET");
    }
}
