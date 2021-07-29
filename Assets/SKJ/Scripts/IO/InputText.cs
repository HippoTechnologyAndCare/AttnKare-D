using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    [SerializeField] private Text txt_Name;
    [SerializeField] private Text txt_Age;
    [SerializeField] private Text txt_Fon;    

    [SerializeField] private InputField inputTxt_Name;
    [SerializeField] private InputField inputTxt_Age;
    [SerializeField] private InputField inputTxt_Fon;

    [SerializeField] private Toggle genderTg_M;
    [SerializeField] private Toggle genderTg_W;
    [SerializeField] private Toggle gradeTg_L;
    [SerializeField] private Toggle gradeTg_H;

    private int currentAge;
    private int minAge;
    private int maxAge;
    private string userInfo;
    private string u_Gender;
    private string u_Grade;

    private void Start()
    {
        minAge = 1;
        maxAge = 99;

        //inputTxt_Name.characterLimit = 5;
        //inputTxt_Age.characterLimit = 2;

        inputTxt_Name.onValueChanged.AddListener(
            (word) => inputTxt_Name.text = Regex.Replace(word, @"[^가-힣]", "")
            );        
        //inputTxt_Age.OnUpdateSelected.AddListener(
        //    (numStr) => inputTxt_Age.text = Regex.Replace(numStr, @"[^0-9]", "")
        //    );
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

    //public void InputFonOnValueChanged()
    //{
    //    if (inputTxt_Fon.GetComponent<InputField>().text == "")
    //        return;

    //    string str = inputTxt_Fon.GetComponent<InputField>().text;

    //    int result = 0;
    //    for (int i = 0; i < str.Length; i++)
    //    {
    //        if (!(int.TryParse(str, out result)))
    //        {
    //            inputTxt_Fon.GetComponent<InputField>().text = "";
    //            return;
    //        }
    //    }
    //}
    private void Update()
    {        

    }

    private string Collect_UserInfo()
    {
        userInfo = txt_Name.text + "_" + txt_Age.text + "_" + u_Gender + "_" + u_Grade + "_" + txt_Fon.text;

        return userInfo;
    }

    public int AgeToInt(string age)
    {
        currentAge = int.Parse(age);
        return currentAge;
    }

    private void Check_Gender()
    {        

        if (genderTg_M != null || genderTg_W == null)
        {                        
            if (genderTg_M.isOn == true)
            {
                u_Gender = "남";
            }

            else
            {
                u_Gender = "여";
            }
        }
    }

    private void Check_Grade()
    {
        if (gradeTg_L != null || gradeTg_H == null)
        {
            if (gradeTg_L.isOn == true)
            {
                u_Grade = "Low";
            }

            else
            {
                u_Grade = "High";
            }
        }
    }

    private void ExceptionHandling_Check()
    {
        string u_N = txt_Name.text;
        string u_A = txt_Age.text;
        string u_P = txt_Fon.text;

        // 각 InputField에 미입력이 있는지 검사
        if (u_N == "" | u_A == "")
        {
            if (u_N == "")
            {
                Debug.Log("이름을 입력하세요!");
            }

            if (u_A == "")
            {
                Debug.Log("나이를 입력하세요!");
            }

            if (u_P == "")
            {
                Debug.Log("전화번호를 입력하세요!");
            }
        }

        // 나이 입력을 올바르게 했는지 검사
        if (u_A != "")
        {
            AgeToInt(u_A);

            if (currentAge < minAge || currentAge > maxAge)
            {
                Debug.Log("나이를 다시 입력하세요!(1~99)");
            }
        }
    }

    public void Confirm_n_Save()
    {
        ExceptionHandling_Check();

        Check_Gender();

        Check_Grade();

        Collect_UserInfo();

        JsonManager.GetInstance().userInformation = userInfo;

        JsonManager.GetInstance().SavePlayerDataToJson();

        JsonManager.GetInstance().StartCoroutine("UploadToCloud");
    }        
}
