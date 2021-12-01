using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TriggerButton : MonoBehaviour
{
    public NumOrderManager Manager;
    public GameObject myButton;
    public GameObject prevButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointerEnter()
    {
        

    }
    public void PointerExit()
    {
        
    }
    public void CursorEnter()
    {
        
        if (myButton == null) //���� �Ҵ�� ��ư�� ���ٸ�
        {
            myButton = Manager.currentButton;
            MoveButton button = myButton.GetComponent<MoveButton>();
            button.Trigger = transform.gameObject;
            button.triggered = true;
            
            return;
        }
        if(myButton!=null) //�ִٸ�
        {
            prevButton = myButton;
            myButton = Manager.currentButton;
            myButton.GetComponent<MoveButton>().Trigger = this.gameObject;
            myButton.GetComponent<MoveButton>().triggered = true;
            
            return;

        }

    }

       
}
