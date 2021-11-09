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
    public TextMeshProUGUI answerText;
    public int answerInt = 0;
    public string[] arrOrder;
    string[] arrAnswer = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "111", "12", "13", "14", "15" };
    bool once;
    Transform colObject;
    Transform _lastCollision;
    Transform collision;
    Grabbable prevGrabbable;
    GameObject prevhitCollision;

    // Start is called before the first frame update
    void Start()
    {
        arrOrder = new string[15];
        once = true;
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
            //  _lastCollision.GetComponent<Collider>().isTrigger = false;
            //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true);
            colObject.gameObject.layer = LayerMask.NameToLayer("boxCard");
            _lastCollision = colObject;
            //  colObject.GetComponent<Collider>().isTrigger = true;

        }
        if (_lastCollision == null)
        {

            _lastCollision = colObject;
            //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true);
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
        
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), false);
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), false);


    }

    public void compareArr()
    {

        
        if (answerInt == 15)
        {


            answerText.text = "�̷��� ������ �ҰԿ�!";
            StartCoroutine(compareAnswer());


        }





    }

    IEnumerator compareAnswer()
    {
        yield return new WaitForSeconds(1.5f);
        if (arrOrder == arrAnswer)
        {

            answerText.text = "����߾��!\n�������� �Ѿ�ϴ�";

        }
        if (arrOrder != arrAnswer)
        {
            answerText.text = "������ �³���? \n�ٽ� �ѹ� Ȯ���غ�����!";

        }
        once = true;

    }

 



}
