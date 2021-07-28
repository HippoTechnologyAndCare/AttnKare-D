using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    [SerializeField]  private InputField inputTxt_Name;
    [SerializeField]  private InputField inputTxt_Age;
    [SerializeField] private Toggle genderTg_M;
    [SerializeField] private Toggle genderTg_W;
    [SerializeField] private Toggle gradeTg_L;
    [SerializeField] private Toggle gradeTg_H;

    private void Start()
    {
        minAge = 1;
        maxAge = 98;

        //inputTxt_Name.characterLimit = 5;
        //inputTxt_Age.characterLimit = 2;

        inputTxt_Name.onValueChanged.AddListener(
            (word) => inputTxt_Name.text = Regex.Replace(word, @"[^가-힣]", "")
            );        
        inputTxt_Age.OnUpdateSelected.AddListener(
            (numStr) => inputTxt_Age.text = Regex.Replace(numStr, @"[^0-9]", "")
            );
    }
    public void InputAgeOnEnd()
    {
        string str = inputTxt_Age.GetComponent<InputField>().text;
        if (str.Length == 1 && str != "0")
        {
            str = "0" + inputTxt_Age.GetComponent<InputField>().text;
            inputTxt_Age.GetComponent<InputField>().text = str;
        }
    }

    public void InputAgeOnValueChanged()
    {
        if (inputTxt_Age.GetComponent<InputField>().text == "")
            return;

        string str = inputTxt_Age.GetComponent<InputField>().text;

        int result = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (!(int.TryParse(str, out result)))
            {
                inputTxt_Age.GetComponent<InputField>().text = "";
                return;
            }
        }
    }

    private void Update()
    {        

    }   

    public void Confirm()
    {
        string u_N = txt_Name.text;
        string u_A = txt_Age.text;

        if (u_N == ""| u_A == "")
        {                        
            if (u_N == "")
            {                
                Debug.Log("이름을 입력하세요!");
            }            
            if (u_A == "")
            {
                Debug.Log("나이를 입력하세요!");
            }            
        }

        if (u_A != "")
        {            
            InputAge(u_A);            
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

    public int InputAge(string age)
    {
        currentAge = int.Parse(age);
        return currentAge;
    }

    public void OutputName()
    {

    }

    public void OutputAge()
    {

    }
}
