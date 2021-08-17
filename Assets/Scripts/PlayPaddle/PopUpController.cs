using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpController : MonoBehaviour
{
    public GameObject Info_PopUp;
    TextMeshProUGUI PopUpText;

    private void Start()
    {
        PopUpText = Info_PopUp.transform.GetComponent<TextMeshProUGUI>();
    }

    public void DoAvtivatePopUp(string msg)
    {
        PopUpText.text = msg;
        StartCoroutine(ShowUp());
    }

    IEnumerator ShowUp()
    {
        Info_PopUp.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        Info_PopUp.SetActive(false);
        PopUpText.text = "";
    }
}
