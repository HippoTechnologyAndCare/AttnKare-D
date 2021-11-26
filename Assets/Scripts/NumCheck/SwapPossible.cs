using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class SwapPossible : MonoBehaviour
{
    public InputBridge xrrig;
    // Start is called before the first frame update
    SnapZone Snapped;
    private void Start()
    {
        Snapped = transform.GetComponent<SnapZone>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (xrrig.RightTrigger < 0.2f)
        {
            if (other.transform.name == "Card")
            {
               
                    CardOut(other);

         

            }


        }
        

    }

    private void CardOut(Collider other)
    {
    
        if (Snapped.HeldItem != other)
        {
            Grabbable prevHeld = Snapped.HeldItem;
            Snapped.ReleaseAll();
            prevHeld.GetComponent<NumCard>().ResetPosRot();
            Snapped.HeldItem = other.transform.GetComponent<Grabbable>();
            transform.GetComponent<TriggerCheck_NumCheck>().TriggerIn();

        }
    
            

    }

    public void CardReset()
    {
        Grabbable prevHeld = Snapped.HeldItem;
        Snapped.ReleaseAll();
        prevHeld.GetComponent<NumCard>().ResetPosRot();
    }
}
