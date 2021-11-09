using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TriggerCard : MonoBehaviour
{
    public InputBridge xrRig;
    public NumCheckManager Manager;
    public string orderNum;
    bool inout;
    int arrNum;


    // Start is called before the first frame update
    void Start()
    {
       
        arrNum = int.Parse(orderNum) - 1;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Necessary"))
        {
            
           //tring num = other.GetComponent<NumCard>().cardNum;
            TriggerCheck(other);

        }
     
       
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Necessary"))
        {

            TriggerExitCheck();

            

        }
    }

    public void TriggerCheck(Collider other)
    {

        other.GetComponent<Rigidbody>().isKinematic = true;
        
        if(xrRig.RightTrigger< 0.2f)
        {
            inout = true;
            Debug.Log(other.name);
            Manager.arrOrder[arrNum] = other.transform.GetComponent<NumCard>().cardNum;
            Manager.answerInt += 1;
            other.transform.GetComponent<NumCard>().SetPosRot(this.transform);
            Debug.Log("IN");
            /* make card number match the order
            if (other.transform.GetComponent<NumCard>().cardNum == orderNum)
            {
                other.transform.GetComponent<NumCard>().SetPosRot(this.transform);


            }
            if(other.transform.GetComponent<NumCard>().cardNum != orderNum)
            {
                other.transform.GetComponent<NumCard>().ResetPosRot();
          

            }
            */
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

    public void TriggerExitCheck()
    {
        if(inout == true)
        {
            Manager.arrOrder[arrNum] = null;
            Manager.answerInt -= 1;

            inout = false;

        }




    }


}
