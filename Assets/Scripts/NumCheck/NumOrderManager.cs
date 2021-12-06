using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;
using UnityEngine.UI;

public class NumOrderManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentButton;
    public InputBridge XrRig;
    public bool active= false;
    public Sprite[] DistracImage;
    public GameObject ImagePrefab;
    public string[] arrOrder;
    public MoveButton[] arrBtn;
    public GameObject[] arrTrig;
    public CheckData_NumCheck dataCheck;
    int sprite = 0;
    int order = 0;
    MoveButton prevCard;
    MoveButton crntCard;

    void Start()
    {
        arrOrder = new string[15];
        string[] arrRandom = new string[arrBtn.Length];
        for (int i = 0; i < arrBtn.Length; i++)
            arrRandom[i] = (i + 1).ToString();  
        ShuffleNum(arrRandom); //shuffle numbers
        for (int count = 0; count < arrBtn.Length; count++){
            string num_s = arrRandom[count];
            int num = int.Parse(num_s);
            arrBtn[count].btnNum = num_s;
            arrBtn[count].SetBtnNum();
            if (num > arrBtn.Length - DistracImage.Length)
                SetSprite(arrBtn[count]);
        }

        StartCoroutine(NextOrder());
    }
    public string[] ShuffleNum(string[] arrNum)
    {
        for (int i = 0; i < arrNum.Length; i++){
            int rnd_n = Random.Range(0, arrNum.Length);
            string temp = arrNum[i];
            arrNum[i] = arrNum[rnd_n];
            arrNum[rnd_n] = temp;
        }
        return arrNum;
    }
    private void SetSprite(MoveButton btn)
    {
        btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        btn.transform.GetChild(0).GetComponent<Image>().sprite = DistracImage[sprite];
        sprite++;

    }

    public void CannotGrab(MoveButton num)
    {
        for(int i =0;i<arrBtn.Length;i++){
            if(arrBtn[i] != num)
                arrBtn[i].enabled = false;
        }
    }

    public void CanGrab()
    {
        for (int i = 0; i < arrBtn.Length; i++)
        {
            arrBtn[i].enabled = true;
        }

    }

    public void CardInTrigger(MoveButton card, TriggerButton trigger)
    {

        if(prevCard==null)
        {
            /*check if card is in correct trigger
             * check if card is inserted in correct order
             * send signal after inserted
             */

        }
        float btnNum_tmp = int.Parse(trigger.trigNum) - 1;
        if(card.btnNum != trigger.trigNum){
            dataCheck.wrongTrigger += 1;
        }
        if(int.Parse(card.btnNum) != int.Parse(prevCard.btnNum+1)){
            dataCheck.wrongorder += 1;
        }
        

    }

    IEnumerator NextOrder()
    {
        if (order == arrTrig.Length + 1){
            GameClear();
            yield break;
        }
        for (int i =0; i <3; i++){
            arrTrig[order].SetActive(true);
            yield return new WaitForSeconds(0.2f);
            arrTrig[order].SetActive(false);
        }
        arrTrig[order].SetActive(true);
        order++;
        
    }

    private void GameClear()
    {
        Debug.Log("clear");
    }


}
