using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanSlotController1 : MonoBehaviour
{
    public GameObject passenger;

    [SerializeField] Transform cube;

    Material mat;


    void Start()
    {
        cube = this.gameObject.transform.Find("Cube");
        mat = cube.GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PLAN")
        {
            mat.color = new Color(0.67f, 0, 0.545f, 0.12f);
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            Debug.Log("enter");
            mat.color = new Color(0.67f, 0, 0.545f, 0.7f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            Debug.Log("exit");
            mat.color = new Color(0.67f, 0, 0.545f, 0.12f);
        }
    }


    public void resetPlanSlot()
    {
        passenger = null;
        mat.color = new Color(0.67f, 0, 0.545f, 0.12f);
    }
}
