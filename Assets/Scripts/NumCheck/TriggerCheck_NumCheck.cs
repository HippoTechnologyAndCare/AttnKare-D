using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TriggerCheck_NumCheck : MonoBehaviour
{
    [Header("Manager")]
    public NumCheckManager Manager;
    public DataCheck_NumMatch DataCheck;
    public string orderNum;
    GameObject card;
    
    int arrNum;
    // Start is called before the first frame update
    void Start()
    {
        arrNum = int.Parse(orderNum) - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerIn()
    {
        card = transform.GetChild(2).gameObject;
        Manager.arrOrder[arrNum] = card.transform.GetComponent<NumCard>().cardNum;
        Manager.answerInt += 1;
        if(orderNum != card.transform.GetComponent<NumCard>().cardNum)
        {
            DataCheck.WrongOrder += 1;
        }
        
        Manager.GetComponent<NumCheckManager>().CompareArr();
    }

    public void TriggerOut()
    {
        Manager.arrOrder[arrNum] = null;
        Manager.answerInt -= 1;
    }
}
