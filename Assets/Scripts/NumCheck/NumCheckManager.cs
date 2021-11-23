using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BNG;

public class NumCheckManager : MonoBehaviour
{
    public GameObject RighthandPointer;
    public Grabber numGrabber;
    public GameObject Triggers;
    public GameObject hitCollision = null;
    public GameObject[] arrCards;
    
    [Header("UI")]
    public Canvas finCanvas;
    public Canvas startCanvas;
    public TextMeshProUGUI answerText;

    [Header("Debug")]
    public int answerInt = 0;
    public string[] arrOrder;
    string[] arrAnswer = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
  //  bool once;
    Transform colObject;
    Transform _lastCollision;
    Transform collision;
    Grabbable prevGrabbable;
    GameObject prevhitCollision;
    IEnumerator currentCoroutine;
    float accumTime = 0f;
    float fadeTime = 1f;
    bool fade;


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

        StartCoroutine(OpeningCoroutine());

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

    public void CompareArr()
    {
        Debug.Log("inside");
        fade = true;
        StartCoroutine(FadeInOut(fade,finCanvas));
        if (answerInt == 15)
        {
            RighthandPointer.SetActive(true);
            answerText.text = "�̷��� ������ �ҰԿ�!";
            



        }
       



    }

    public void ResetCoroutine()
    {
        StopAllCoroutines();

    }    
    public void startCoroutines()
    {
        StartCoroutine(FinishAnswer());

    }

    public IEnumerator FinishAnswer()
    {
        yield return new WaitForSeconds(1.5f);
        
        for(int i = 0; i < arrOrder.Length; i++)
        {
            if(!arrCards[i].Equals(arrAnswer[i]))
            {
                Debug.Log("not equal");
                answerText.text = "������ �³���? \n�ٽ� �ѹ� Ȯ���غ�����!";
                yield return new WaitForSeconds(3.0f);
                fade = false;
                yield return StartCoroutine(FadeInOut(fade, finCanvas));
                yield break;
            }
        }
        answerText.text = "����߾��!\n�������� �Ѿ�ϴ�";
            
        
        ResetCoroutine();
      //  once = true;

    }
    public IEnumerator AgainAnswer()
    {
        yield return new WaitForSeconds(0.9f);
        fade = false;
        yield return StartCoroutine(FadeInOut(fade, finCanvas));

    }

    IEnumerator OpeningCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        accumTime = 0f;

        fade = false;
        yield return StartCoroutine(FadeInOut(fade,startCanvas)); 
        yield return new WaitForSeconds(0.8f);
        RighthandPointer.SetActive(false);


        for (int i =0;i <4;i++)
        {
            Triggers.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Triggers.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            
        }

        Triggers.SetActive(true);
    }

 
    

    private IEnumerator FadeInOut(bool fade,Canvas canvas) // true = fade in / false = fade out
    {
        if(fade)//fade in
        {
            accumTime = 0f;

            while (accumTime < fadeTime)
            {
                canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
                yield return 0;
                accumTime += Time.deltaTime;
            }
            canvas.GetComponent<CanvasGroup>().alpha = 1f;

        }
        if(!fade)//fade out
        {
            while (accumTime < fadeTime)
            {
                canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
                yield return 0;
                accumTime += Time.deltaTime;
            }
            canvas.GetComponent<CanvasGroup>().alpha = 0f;

        }

    }
}
