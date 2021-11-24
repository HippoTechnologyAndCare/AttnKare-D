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
        string[] arrNum = new string[arrCards.Length];
        

        for (int i = 0; i < arrCards.Length-4; i++)
        {
            arrNum[i] = (i + 1).ToString();
        }

        arrNum[arrCards.Length - 4] = ":)";
        arrNum[arrCards.Length - 3] = "&";
        arrNum[arrCards.Length - 2] = "$";
        arrNum[arrCards.Length - 1] = "@";

        ShuffleNum(arrNum); //shuffle numbers

        for (int count = 0; count < arrCards.Length; count++)
        {
            
            string num_s = arrNum[count];

            arrCards[count].GetComponent<NumCard>().cardNum = num_s;
            arrCards[count].GetComponent<NumCard>().SetCardNum();
            Debug.Log(arrCards[count].transform.name);

        }

        

    }

    private void Update()
    {
       
    }

    public string[] ShuffleNum(string[] arrNum)
    {
        for(int i =0; i < arrNum.Length; i++)
        {
            int rnd_n = Random.Range(0, arrNum.Length);
            string temp = arrNum[i];
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
            answerText.text = "이렇게 마무리 할게요!";
        }
      
    }

    public void ResetCoroutine()
    {
        StopAllCoroutines();
    }    
    public void CoroutineWrapper(string name)
    {
        string corname = name;
        StartCoroutine(corname);

    }

    public IEnumerator FinishAnswer()
    {
        yield return new WaitForSeconds(1.5f);
        
        for(int i = 0; i < arrOrder.Length; i++)
        {
            if(!arrCards[i].Equals(arrAnswer[i]))
            {
                Debug.Log("not equal");
                answerText.text = "정답이 맞나요? \n다시 한번 확인해보세요!";
                yield return new WaitForSeconds(3.0f);
                fade = false;
                yield return StartCoroutine(FadeInOut(fade, finCanvas));
                yield break;
            }
        }
        answerText.text = "고생했어요!\n다음으로 넘어갑니다";
        RighthandPointer.SetActive(false);

        ResetCoroutine();

    }
    public IEnumerator AgainAnswer()
    {
        yield return new WaitForSeconds(0.9f);
        fade = false;
        yield return StartCoroutine(FadeInOut(fade, finCanvas));
    }

    public IEnumerator OpeningCoroutine()
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
            canvas.gameObject.SetActive(true);
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
            canvas.gameObject.SetActive(false);

        }

    }
}
