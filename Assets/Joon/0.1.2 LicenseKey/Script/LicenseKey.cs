using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;
using System.IO;
using UnityEngine.SceneManagement;


namespace LicenseKey
{
    public class LicenseKey : MonoBehaviour
    {
        public HUD HUD;
        string m_sCurrentMac;
        private string m_sMacAddress;
        private string m_sLicenseKeyPath = ""; //제품의 LicenseKeyPath
        private string m_sLicenseKey = "";
        private string FilePath_Root = "";
        private void Awake()
        {
         //   FilePath_Root = Application.persistentDataPath + "/" + "Licensekey/"; //등록된 컴퓨터인지 여부 (LocalLow)
            FilePath_Root = Application.streamingAssetsPath + "/AttnKare-D/"; //등록된 컴퓨터인지 여부 (LocalLow)
            m_sCurrentMac = FilePath_Root + "RegisteredMacAddress.txt";
            m_sLicenseKeyPath = FilePath_Root + "LicenseKey.txt"; //제품 LicenseKey
            m_sMacAddress = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();  
        }
        
        private void CreateFolder()
        {
            if (!Directory.Exists(FilePath_Root))
            {
                Directory.CreateDirectory(FilePath_Root);
            }
        }
        
        void CompareMacAddr()
        {
            if (!Directory.Exists(FilePath_Root)) { HUD.PopUP("MacAddress"); return; }
            StreamReader reader = new StreamReader(m_sCurrentMac);
            string m_sRegisteredMacAddress = reader.ReadToEnd(); m_sRegisteredMacAddress = m_sRegisteredMacAddress.Replace("\r\n", string.Empty); Debug.Log(m_sRegisteredMacAddress);
            if (m_sRegisteredMacAddress == m_sMacAddress) { SceneManager.LoadScene("LOGIN"); }
            else HUD.PopUP("MacAddress");
        }
        void Start()
        {
            GetLicenseKey();
            Debug.Log(m_sMacAddress);
            if (PlayerPrefs.HasKey("LicenseKey")){
                CompareMacAddr();
            }
            else { Debug.Log("NOT REGISTERED License"); return; }
        }
        public void CheckLicenseKey(string m_sInputKey)
        {
            Debug.Log(m_sLicenseKey);
            if (m_sInputKey == m_sLicenseKey) { RegisterPC(); return; }
            if (m_sInputKey != m_sLicenseKey) { HUD.PopUP("LicenseKey"); return; }
        }
        private void GetLicenseKey()
        {
            StreamReader reader = new StreamReader(m_sLicenseKeyPath);
            m_sLicenseKey = reader.ReadToEnd(); m_sLicenseKey = m_sLicenseKey.Replace("\r\n", string.Empty); Debug.Log(m_sLicenseKey);
        }
        
        private void RegisterPC()
        {
            CreateFolder();
            PlayerPrefs.SetString("LicenseKey", m_sLicenseKey);
            File.WriteAllText(m_sCurrentMac, m_sMacAddress);
            SceneManager.LoadScene("LOGIN");
        } 
    }
}
