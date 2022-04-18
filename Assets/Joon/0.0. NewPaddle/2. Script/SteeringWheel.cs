using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class SteeringWheel : MonoBehaviour
{
    public Transform Handle; //grabbable handle

    Grabbable grab;
    public GameObject Vehicle;
    public Rigidbody VehicleRigidbody;

    [SerializeField] private Transform LeftHandModel;
    private bool m_bLeftHand;
    [SerializeField] private Transform RighthandModel;
    private bool m_bRightHand;

    public float CurrentWheelRotation =0;
    public Transform directionalObject;

    private float turnDampening; //if higher, ship will perfectly follow steering at a 1:1 ratio
                                 //if lower, lag down a little bit(smoother)

    public Transform Directionalobject; //place at the middle of wheel(Áß½É)
    [SerializeField] private bool Grabbed;

    // Start is called before the first frame update
    void Start()
    {
        VehicleRigidbody = Vehicle.GetComponent<Rigidbody>();
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
    }
    void ConverthandRotationToStreeringWheelRotation()
    {
        if (m_bLeftHand) {  }//Quaternion newRot= Quaternion.Euler(0,0,}
        if (m_bRightHand) { }
    }
    void TurnVehicle()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "Grabber")
        {
            Grabbed = true;
        }
    }
}
