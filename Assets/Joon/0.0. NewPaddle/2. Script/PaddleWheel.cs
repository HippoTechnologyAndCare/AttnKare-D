using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class PaddleWheel: MonoBehaviour
{
   // public Transform Handle; //grabbable handle

    Grabbable grab;

    [SerializeField] private Transform LeftHandModel;
    private bool m_bLeftHand;
    [SerializeField] private Transform RighthandModel;
    private bool m_bRightHand;

    public float CurrentWheelRotation =0;

    private float turnDampening; //if higher, ship will perfectly follow steering at a 1:1 ratio
                                 //if lower, lag down a little bit(smoother)

    public Transform Directionalobject; //place at the middle of wheel(Áß½É)
    [SerializeField] private bool Grabbed;

    // Start is called before the first frame update
    void Start()
    {
        grab = GetComponent<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        HandsOnWheel();
        ConverthandRotationToStreeringWheelRotation();
        TurnVehicle();
        CurrentWheelRotation = -transform.rotation.eulerAngles.z;
    }

    void HandsOnWheel()
    {
        if (grab != null && grab.BeingHeld)
        {
            m_bLeftHand = LeftHandModel.childCount > 0;
            m_bRightHand = RighthandModel.childCount > 0;
        }
        if(grab== null || !grab.BeingHeld)
        {
            m_bLeftHand = m_bRightHand = false;
        }
    }
    void ConverthandRotationToStreeringWheelRotation()
    {
        if (m_bLeftHand) { Quaternion newRot = Quaternion.Euler(0, 0, LeftHandModel.transform.rotation.eulerAngles.z);
            Directionalobject.rotation = newRot;
            transform.parent = Directionalobject;
        }//Quaternion newRot= Quaternion.Euler(0,0,}
        if (m_bRightHand) {
            Quaternion newRot = Quaternion.Euler(0, 0, RighthandModel.transform.rotation.eulerAngles.z);
            Directionalobject.rotation = newRot;
            transform.parent = Directionalobject;
        }
    }
    void TurnVehicle()
    {

    }
    private void OnTriggerStay(Collider other)
    {

    }
}
