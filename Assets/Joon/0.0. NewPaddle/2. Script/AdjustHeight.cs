using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    public Transform floor;
    public Transform CenterEye;
    public Transform MidPoint;
    int a;
    int b;
    int c;
    float m_fMid;
    float m_fFloor;
    public bool m_bHeight = true;
    // Start is called before the first frame update

    private void Awake()
    {
        m_fMid = Mathf.Abs(CenterEye.position.y - MidPoint.position.y);
        m_fFloor = Mathf.Abs(floor.position.y - MidPoint.position.y);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
