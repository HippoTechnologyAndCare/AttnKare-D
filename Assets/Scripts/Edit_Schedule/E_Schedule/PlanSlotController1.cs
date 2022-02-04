using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanSlotController1 : MonoBehaviour
{
    public GameObject passenger;

    Image image;


    void Start()
    {
        image = this.GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PLAN")
        {            
            image.color = new Color(0, 255, 0, 0.5f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            Debug.Log("enter");
            image.color = new Color(0, 255, 0, 0.5f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            image.color = new Color(255, 255, 255, 0.15f);
        }
    }


    public void resetPlanSlot()
    {
        passenger = null;
        image.color = new Color(255, 255, 255, 0.15f);
    }
}
