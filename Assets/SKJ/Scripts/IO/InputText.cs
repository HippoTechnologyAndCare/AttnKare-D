using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    [SerializeField]  private Text txt_Name;
    [SerializeField]  private Text txt_Age;

    private int currentAge;
    private int minAge;
    private int maxAge;
    private string userInfo;

    [SerializeField]  private InputField inputTxT_Name;
    [SerializeField]  private InputField inputTxt_Age;

    private void Start()
    {
        minAge = 1;
        maxAge = 99;
    }

    private void Update()
    {
        
        
    }

    public void Confirm()
    {
        if (txt_Name == null || txt_Age == null)
        {
            if(txt_Name == null)
            {
                Debug.Log("이름을 입력하세요!");
            }

            if (txt_Age == null)
            {
                Debug.Log("나이를 입력하세요!");
            }
            return;
        }

        if (txt_Age != null)
        {
            InputAge();

            if (currentAge < minAge || currentAge > maxAge)
            {
                Debug.Log("나이를 다시 입력하세요!(1~99)");
            }
        }

        JsonManager.GetInstance().SavePlayerDataToJson(userInfo);
    }
    public void InputName()
    {
        
    }

    public void InputAge()
    {
        currentAge = int.Parse(inputTxt_Age.text);
    }

    public void OutputName()
    {

    }

    public void OutputAge()
    {

    }
}
