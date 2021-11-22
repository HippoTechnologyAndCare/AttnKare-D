using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BNG;

public class NumCheckManager : MonoBehaviour
{
    public Grabber numGrabber;
    public GameObject[] arrCards;
    public GameObject hitCollision = null;
    public Canvas finCanvas;
    public TextMeshProUGUI answerText;
    public int answerInt = 0;
    public string[] arrOrder;
    string[] arrAnswer = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "111", "12", "13", "14", "15" };
  //  bool once;
    Transform colObject;
    Transform _lastCollision;
    Transform collision;
    Grabbable prevGrabbable;
    GameObject prevhitCollision;
    IEnumerator currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
      
        arrOrder = new string[15];
     //   once = true;
        int[] arrNum = new int[arrCards.Length];
        

        for (int i = 0; i < arrCards.Length; i++)
        {
            arrNum[i] = i + 1;
        }

        ShuffleNum(arrNum); //shuffle numbers

        for (int count = 0; count < arrCards.Length; count++)
        {
            int num = arrNum[count];
            string num_s = num.ToString();

            arrCards[count].GetComponent<NumCard>().cardNum = num_s;
            arrCards[count].GetComponent<NumCard>().SetCardNum();
            Debug.Log(arrCards[count].transform.name);

        }

    }

    private void Update()
    {
       
    }

    public int[] ShuffleNum(int[] arrNum)
    {

        for(int i =0; i < arrNum.Length; i++)
        {
            int rnd_n = Random.Range(0, arrNum.Length);
            int temp = arrNum[i];
            arrNum[i] = arrNum[rnd_n];
            arrNum[rnd_n]=temp;

        }
        return arrNum;

    }

    public void DisableCollision(Transform colObject)
    {

        
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true );
        if (_lastCollision != null && _lastCollision != colObject)
        {
           
            colObject.gameObject.layer = LayerMask.NameToLayer("boxCard");
            _lastCollision = colObject;
        

        }
        if (_lastCollision == null)
        {

            _lastCollision = colObject;
            colObject.gameObject.layer = LayerMask.NameToLayer("boxCard");
        }
        else
        {
            return;
        }



    }

    public void getGrabbable()
    {

        prevGrabbable = numGrabber.HeldGrabbable;

    }
    public void EnableCollision()
    {
      
        if(prevGrabbable && prevGrabbable.CompareTag( "Necessary") )
        {
            prevGrabbable.gameObject.layer = LayerMask.NameToLayer("numCard");

            return;
        }
        if(hitCollision && prevhitCollision != hitCollision)
        {
            Debug.Log("enter");
            hitCollision.layer = LayerMask.NameToLayer("numCard");
            prevhitCollision = hitCollision;
            hitCollision = null;
            return;
        }
       

    }

    public void compareArr()
    {
        Debug.Log("inside");


        if (answerInt == 15)
        {
            answerText.text = "이렇게 마무리 할게요!";
            finCanvas.gameObject.SetActive(true);



        }
       



    }

    public void ResetCoroutine()
    {
        StopAllCoroutines();

    }    
    public void startCoroutines()
    {
        StartCoroutine(CompareAnswer());

    }

    public IEnumerator CompareAnswer()
    {
       
        

        yield return new WaitForSeconds(1.5f);
        if (arrOrder == arrAnswer)
        {

            answerText.text = "고생했어요!\n다음으로 넘어갑니다";

        }
        if (arrOrder != arrAnswer)
        {
            answerText.text = "정답이 맞나요? \n다시 한번 확인해보세요!";
            yield return new WaitForSeconds(3.0f);
            finCanvas.gameObject.SetActive(false);



        }

        ResetCoroutine();
      //  once = true;

    }

 



}
