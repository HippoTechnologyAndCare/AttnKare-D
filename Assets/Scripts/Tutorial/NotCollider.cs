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
    Collider m_collider;
    void Start()
    {
        int a;
        m_collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        m_goCloset = ClosestGrabber.ClosestGrabbable ? ClosestGrabber.ClosestGrabbable.gameObject : null;
        m_goClosestRemote = ClosestGrabber.ClosestRemoteGrabbable ? ClosestGrabber.ClosestRemoteGrabbable.gameObject : null;
        m_goClosestRemote = Grabber.HeldGrabbable ? Grabber.HeldGrabbable.gameObject : null;
        if (m_goClosestRemote == BANANA || m_goCloset == BANANA)
        {
            m_collider.enabled = false;
        }
        if(m_goClosestRemote == null && m_goCloset == null && m_goGrabbed != BANANA)
        {
            m_collider.enabled = true;
        }
    }
}
