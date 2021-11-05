using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TriggerCard : MonoBehaviour
{
    public InputBridge xrRig;
    public string orderNum;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        string num= other.GetComponent<NumCard>().cardNum;
        TriggerCheck(other);
       
    }
    public void TriggerCheck(Collider other)
    {
        if(xrRig.RightTrigger< 0.2f)
        {
            Debug.Log("IN");
            if (other.transform.GetComponent<NumCard>().cardNum == orderNum)
            {
                other.transform.GetComponent<NumCard>().SetPosRot(this.transform);


            }
            if(other.transform.GetComponent<NumCard>().cardNum != orderNum)
            {
                other.transform.GetComponent<NumCard>().ResetPosRot();
          

            }


            /*
            if (other.transform.GetComponent<NumCard>().tag == "Necessary")
            {
                other.transform.GetComponent<NumCard>().SetPosRot(this.transform);


            }
            if(other.transform.GetComponent<NumCard>().tag != "Necessary")
            {
                other.transform.GetComponent<NumCard>().ResetPosRot();
          

            }
            */




        }
    }


}
