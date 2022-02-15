using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanSlotController1 : MonoBehaviour
{
    public GameObject passenger;

    public bool isStore;

    [SerializeField] Transform cube;

    MeshRenderer mesh;


    void Start()
    {
        isStore = false;
        cube = this.gameObject.transform.Find("Cube");
        mesh = cube.GetComponent<MeshRenderer>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "PLAN" && !PlanCubeController1.working)
    //    {
    //        mat.color = new Color(0.67f, 0, 0.545f, 0.12f);

    //    }
    //}

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == "PLAN")
    //    {
    //        Debug.Log("enter");
    //        mat.color = new Color(0.67f, 0, 0.545f, 0.7f);
    //    }
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider.tag == "PLAN")
    //    {
    //        Debug.Log("exit");
    //        mat.color = new Color(0.67f, 0, 0.545f, 0.12f);
    //    }
    //}

    private void Update()
    {
        if(passenger == null && isStore)
        {
            isStore = false;
            mesh.enabled = true;
        }
    }


    public void resetPlanSlot()
    {
        passenger = null;
        //mat.color = new Color(0.67f, 0, 0.545f, 0.12f);
    }
}



