using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BNG;

public class MoveButton : MonoBehaviour
{
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public NumOrderManager Manager;
    public GameObject Trigger;
    public bool triggered;
    bool click;
    GameObject prevButton;
    RectTransform rect;
    Vector3 OrginPos;

    Transform parentCursor;
    // Start is called before the first frame update
    void Start()
    {
        parentCursor = RighthandPointer.GetComponent<UIPointer>()._cursor.transform;
        OrginPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if(click)
        {
            if(RighthandPointer.GetComponent<LineRenderer>().enabled == true)
            {
                if(XrRig.RightTrigger > 0.5f)
                {
                    transform.position = new Vector3(parentCursor.position.x, parentCursor.position.y, transform.position.z);
                }
                if(XrRig.RightTrigger<0.2f)
                {
                    if (!triggered)
                    {
                        ResetButton();
                        
                    }
                    if(triggered)
                    {
                        SetButton(Trigger.transform.position);
                    }
                    
                }
                    
            }
            
        }
        
    }

    public void dataTest()
    {
        Debug.Log("test");
    }
    public void MoveNumber()
    {
        click = true;
        Manager.currentButton = this.transform.gameObject;
    }

    public void ResetButton()
    {
        click = false;
        Manager.currentButton = null;
        transform.position = OrginPos;
    }

    public void SetButton(Vector3 pos)
    {
        click = false;
        transform.position = pos;
        Manager.currentButton = null;
        Manager.ButtonInTrigger = false;
        if (Trigger.GetComponent<TriggerButton>().prevButton == null)
        {
            return;
        }
        if (Trigger.GetComponent<TriggerButton>().prevButton != null)
        {
            prevButton = Trigger.GetComponent<TriggerButton>().prevButton;
            prevButton.GetComponent<MoveButton>().ResetButton();
            Trigger.GetComponent<TriggerButton>().prevButton = null;

        }
        
        



    }
   

}
