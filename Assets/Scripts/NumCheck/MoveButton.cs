using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using BNG;

public class MoveButton : MonoBehaviour
{
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public NumOrderManager Manager;
    public GameObject Trigger;
    public bool triggered;
    public bool click;
    public string btnNum;
    GameObject prevButton;
    RectTransform rect;
    Vector3 OrginPos;
    TextMeshProUGUI btnText;

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
                    transform.position = new Vector3(parentCursor.position.x, parentCursor.position.y, transform.position.z);
                if(XrRig.RightTrigger < 0.2f){
                    if (!triggered)
                        ResetButton();
                    click = false;
                }   
            }
        }
    }

    public void SetBtnNum(){
        btnText = transform.GetComponentInChildren<TextMeshProUGUI>();
        btnText.text = btnNum;
    }
    public void GrabButton(){
        click = true;
        Manager.currentButton = this.transform.gameObject;
        Manager.CannotGrab(int.Parse(btnNum));
    }
    

    public void ResetButton(){
        Trigger = null;
        triggered = false;
        transform.position = OrginPos;
        if(Manager.active == false)
            Manager.currentButton = null;
    }

    public void SetButton(){ 
        transform.position = Trigger.transform.position;
        Manager.CanGrab();
        Manager.currentButton = null; 
    }
   

}
