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

    int sprite = 0;


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
        /*
        GameObject image = Instantiate(ImagePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        image.transform.SetParent(card.GetComponentInChildren<Canvas>().transform);
        RectTransform imageRect = image.GetComponent<RectTransform>();
        imageRect.anchoredPosition3D = new Vector3(0, 0, 0);
        imageRect.localEulerAngles = new Vector3(0, 0, -90);
        imageRect.localScale = new Vector3(1, 1, 1);
        imageRect.sizeDelta = new Vector2(0.11f, 0.17f);
        image.GetComponent<Image>().sprite = DistracImage[sprite];
        */
        btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        btn.transform.GetChild(0).GetComponent<Image>().sprite = DistracImage[sprite];
        sprite++;

    }

    public void CannotGrab(int num)
    {
        for(int i =0;i<arrBtn.Length;i++)
        {
            if(i != num)
            {
                arrBtn[i].enabled = false;

            }
        }
        //arrBtn[num].enabled = false;
    }

    public void CanGrab()
    {
        for (int i = 0; i < arrBtn.Length; i++)
        {
            arrBtn[i].enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {


        
    }
}
