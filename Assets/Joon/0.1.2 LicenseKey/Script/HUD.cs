using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace LicenseKey
{
    public class HUD : MonoBehaviour
    {
        public LicenseKey Manager;
        public InputField input_LicenseKey;
        public GameObject go_Error;
        public void PopUP(string sKey)
        {
            Color m_colorText = go_Error.GetComponent<Text>().color;
            m_colorText.a = 0; string m_sPopup = "";
            switch (sKey)
            {
                case "LicenseKey": m_sPopup = "�߸��� License Key�� �Է��ϼ̽��ϴ�."; break;
                case "MacAddress": m_sPopup = "�ش� ��ǰ�� �ٸ� PC�� ��ϵǾ��ֽ��ϴ�."; break;
            }
            go_Error.GetComponent<Text>().text = m_sPopup;
            go_Error.SetActive(true);
            go_Error.GetComponent<Text>().DOFade(1, 0.6f);
        }
        public void SendLicenseKey()
        {
            string LicenseKey = input_LicenseKey.text;
            Manager.CheckLicenseKey(LicenseKey);
        }
    }
}
