using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    public Transform floor;
    public Transform CenterEye;
    public Transform MidPoint;
    float m_fMid;
    float m_fFloor;
    public bool m_bHeight = true;
    // Start is called before the first frame update
    float height;
    private void Awake()
    {
       
    }
    void Start()
    {
        height = GetHeight.HEIGHT;
        m_fFloor = MidPoint.position.y - floor.position.y;
        float f = m_fFloor - height;
        float y = height - m_fFloor;
        if (height > m_fFloor) {floor.position = new Vector3(floor.position.x, floor.position.y - (height - m_fFloor), floor.position.z); return; }
        if (height < m_fFloor) floor.position = new Vector3(floor.position.x, floor.position.y + (m_fFloor - height), floor.position.z);
        Debug.Log(height + "  " + m_fFloor + "  " + f + "   " + y + "  ");
    }

    // Update is called once per frame
    void Update()
    {/*
        if (m_bHeight)
        {

            if (m_fMid < m_fFloor)
            {
                floor.position = new Vector3(floor.position.x, floor.position.y+0.01f, floor.position.z);
                m_fMid = Mathf.Abs(CenterEye.position.y - MidPoint.position.y);
                m_fFloor = Mathf.Abs(floor.position.y - MidPoint.position.y);
                if (m_fMid >= m_fFloor) m_bHeight = false;
            }
            if (m_fMid > m_fFloor)
            {
                floor.position = new Vector3(floor.position.x, floor.position.y - 0.01f, floor.position.z);
                m_fMid = Mathf.Abs(CenterEye.position.y - MidPoint.position.y);
                m_fFloor = Mathf.Abs(floor.position.y - MidPoint.position.y);
                if (m_fMid <= m_fFloor) m_bHeight = false;
            }
            
        }
        */
    }
}
