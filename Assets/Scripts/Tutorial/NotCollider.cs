using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class NotCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BANANA;
    public GrabbablesInTrigger ClosestGrabber;
    public Grabber Grabber;
    private GameObject m_goGrabbed;
    private GameObject m_goClosestRemote;
    private GameObject m_goCloset;
    CapsuleCollider m_collider;
    void Start()
    {
        m_collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        m_goCloset = ClosestGrabber.ClosestGrabbable.gameObject;
        m_goClosestRemote = ClosestGrabber.ClosestRemoteGrabbable.gameObject;
        m_goGrabbed = Grabber.HeldGrabbable.gameObject;
        if (m_goClosestRemote == BANANA || m_goCloset == BANANA)
        {
            m_collider.enabled = false;
        }
        if(m_goClosestRemote != BANANA && m_goCloset != BANANA && m_goGrabbed != BANANA)
        {
            m_collider.enabled = true;
        }
    }
}
