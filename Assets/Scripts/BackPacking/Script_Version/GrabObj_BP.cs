using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class GrabObj_BP : MonoBehaviour
{
    // Start is called before the first frame update
    public Object_BP.OBJ_BP m_eObj;
    public Object_BP.KIND_BP m_eKind;

    Vector3 m_v3Start;
    void Start()
    {
        m_v3Start = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Reset()
    {
        transform.position = m_v3Start;
    }
}
