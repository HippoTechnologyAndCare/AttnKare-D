using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanSlotController1 : MonoBehaviour
{
    public GameObject passenger;

    Color thisColor;


    void Start()
    {
        thisColor = this.GetComponent<Image>().color;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            thisColor = new Color(0, 255, 0, 0.5f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            thisColor = new Color(255, 255, 255, 0.15f);
        }
    }


    public void resetPlanSlot()
    {
        passenger = null;
        thisColor = new Color(255, 255, 255, 0.15f);
    }
}
