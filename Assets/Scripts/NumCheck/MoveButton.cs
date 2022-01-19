using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using BNG;

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public NumOrderManager Manager;
    public GameObject Trigger;
    public bool triggered;
    public bool click;
    public string btnNum;
    public string color = "#FF0000";
    public bool distraction = false;
    Transform Canvas;
    Transform originalParent;
    GameObject prevButton;
    RectTransform rect;
    Vector3 OrginPos;
    TextMeshProUGUI btnText;
    Color activatedColor;
    Color originalColor;
    Transform parentCursor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = btnText.color;
        ColorUtility.TryParseHtmlString(color, out activatedColor);
        parentCursor = RighthandPointer.GetComponent<UIPointer>()._cursor.transform;
        OrginPos = transform.position;
        originalParent = this.transform.parent;
        Canvas = this.transform.parent.parent;
        Debug.Log(Canvas.name);
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
                if (XrRig.RightTrigger < 0.2f) {
                    Debug.Log("up");
                    this.transform.SetParent(originalParent);
                    if (Trigger) { Manager.CardInTrigger(this, Trigger.GetComponent<TriggerButton>()); }
                    if(!Trigger) ResetButton();
                    click = false;
                    Manager.CanGrab();
                }
                }
            if(RighthandPointer.GetComponent<LineRenderer>().enabled == false)
            {
                ResetButton();
                Manager.CanGrab();
                Manager.currentButton = null;
                click = false;
            }
            
        }
    }

    public void SetBtnNum(){
        transform.name = btnNum;
        btnText = transform.GetComponentInChildren<TextMeshProUGUI>();
        btnText.text = btnNum;
        
    }

    public void OnPointerDown(PointerEventData pointerEventData){

        this.transform.SetParent(Canvas);
        if(distraction)
        {
            Manager.GetComponent<CheckData_NumCheck>().distractedBy += Time.deltaTime;
        }
        click = true;
        btnText.color = activatedColor;
        Manager.currentButton = this.transform.gameObject;
        Manager.CannotGrab(transform.GetComponent<MoveButton>());
    }
  
    public void ResetButton(){
        Trigger = null;
     //   triggered = false;
        transform.position = OrginPos;
        btnText.color = originalColor;
        if(Manager.active == false)
            Manager.currentButton = null;
    }

    public void SetButton(){ 
        transform.position = Trigger.transform.position;
        btnText.color = originalColor;
        Manager.CanGrab();
        Manager.currentButton = null; 
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("inside");
        if(collision.gameObject.tag == "Necessary")
        {
            Manager.active = true;
      //      triggered = true;
            Trigger = collision.gameObject;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Necessary")
        {
    //        triggered = false;
            Trigger = null;
        }

    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        




    }



}
