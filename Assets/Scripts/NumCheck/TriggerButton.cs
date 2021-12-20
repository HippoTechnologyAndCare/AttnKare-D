using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;

public class TriggerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public NumOrderManager Manager;
    public GameObject crnt;
    public GameObject prev;
    public GameObject temp;
    public string trigNum;
    int trigNum_tmp;
    private void Start()
    {
        trigNum_tmp = int.Parse(trigNum) - 1;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(temp)
        {
            temp.GetComponent<MoveButton>().triggered = false;
            temp = null;
        }
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log(Manager.currentButton);
        if(Manager.currentButton!=null)
        {
            Manager.active = true;
            temp = Manager.currentButton;
            temp.GetComponent<MoveButton>().triggered = true;
            temp.GetComponent<MoveButton>().Trigger = this.gameObject;
        }
    }
       

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (temp)
        {
            if (temp.GetComponent<MoveButton>().click == false)
            {
                crnt = temp;
                MoveButton button = crnt.GetComponent<MoveButton>();
                Manager.CardInTrigger(button, this);
            }
        }
    }    
}
