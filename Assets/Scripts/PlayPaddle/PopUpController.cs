using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpController : MonoBehaviour
{
    public GameObject PopUp;
    TextMeshProUGUI PopUpText;

    private void Start()
    {
        PopUpText = PopUp.transform.GetComponent<TextMeshProUGUI>();
    }

    public void DoAvtivatePopUp(string msg)
    {
        PopUpText.text = msg;
        StartCoroutine(ShowUp());
    }

    IEnumerator ShowUp()
    {
        PopUp.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        PopUp.SetActive(false);
        PopUpText.text = "";
    }
}
