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

    public string btnNum;
    int btnNum_tmp;
    // Start is called before the first frame update

    private void Start()
    {
        btnNum_tmp = int.Parse(btnNum) - 1;
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
       
    /*
    public void CursorStay()
    {
        SetPosButton();
    }

    private void SetPosButton()
    {
        if (temp.GetComponent<MoveButton>().click == false)
        {
            if (crnt != null) //있다면
            {
                prev = crnt;
                prev.GetComponent<MoveButton>().ResetButton();

            }
            Debug.Log("passed");
            crnt = temp;
            MoveButton button = crnt.GetComponent<MoveButton>();
            button.SetButton();
            Manager.active = false;

        }
    }
    */
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (temp.GetComponent<MoveButton>().click == false)
        {
            if (crnt != null) //있다면
            {
                prev = crnt;
                prev.GetComponent<MoveButton>().ResetButton();

            }
            crnt = temp;
            MoveButton button = crnt.GetComponent<MoveButton>();
            Manager.arrOrder[btnNum_tmp] = button.btnNum;
            button.SetButton();
            Manager.active = false;

        }
    }    


}
