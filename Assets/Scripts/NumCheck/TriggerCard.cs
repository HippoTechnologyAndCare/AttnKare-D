using BNG;
using UnityEngine;

public class TriggerCard : MonoBehaviour
{
    public InputBridge xrRig;
    public NumCheckManager Manager;
    public string orderNum;
    GameObject card;
  //bool alrdyIN = false;
    bool inout = false;
    int arrNum;
    bool alrdyIn = false;


    // Start is called before the first frame update
    void Start()
    {
       
        arrNum = int.Parse(orderNum) - 1;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Necessary") )
        {

            TriggerCheck(other);
           
        }
     
       
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Necessary"))
        {
            if(other.gameObject == card)
            {
                TriggerExitCheck();
            }
            
        }
    }

    public void TriggerCheck(Collider other)
    {
    if (xrRig.RightTrigger < 0.2f)
    {
        //   other.GetComponent<Rigidbody>().isKinematic = true;
        other.GetComponent<Rigidbody>().useGravity = false;
        Manager.arrOrder[arrNum] = other.transform.GetComponent<NumCard>().cardNum;
        Manager.answerInt += 1;
        other.transform.GetComponent<NumCard>().SetPosRot(this.transform);
        card = other.gameObject;
        inout = true;
        Manager.GetComponent<NumCheckManager>().compareArr();
        Debug.Log("IN");
    }


    }

    public void TriggerExitCheck()
    {
        if (inout){
            Manager.arrOrder[arrNum] = null;
            Manager.answerInt -= 1;
            // card = null;
            inout = false;

        }
           


    }


}
