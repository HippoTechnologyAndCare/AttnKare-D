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
        private string m_sKeyPath;
        string path = "/LicenseKey.txt";
        private string m_sMacAddress;
        private string m_sLicenseKey = "qwertyuiopasdfghjklzxcvbn";
        private void Awake()
        {
            m_sKeyPath = Application.streamingAssetsPath;
            m_sMacAddress = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
            path = m_sKeyPath + path;

        }

        void CompareMacAddr()
        {
            StreamReader reader = new StreamReader(path);
            string m_sCurrentMac = reader.ReadToEnd(); m_sCurrentMac = m_sCurrentMac.Replace("\r\n", string.Empty); Debug.Log(m_sCurrentMac);
            if (m_sCurrentMac == m_sMacAddress) { SceneManager.LoadScene(0); }
        }
        void Start()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            Debug.Log(m_sMacAddress);
            if (File.Exists(path))
            {
                CompareMacAddr();
            }
            else { Debug.Log("NOT REGISTERED"); return; }
        }
        public void CheckLicenseKey(string m_sInputKey)
        {
            if (m_sInputKey == m_sLicenseKey) { RegisterPC();  SceneManager.LoadScene(0); return; }
            if (m_sInputKey != m_sLicenseKey) { HUD.PopUP(); return; }
        }
        
        private void RegisterPC()
        {
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(m_sMacAddress);
            writer.Close();
        }
    }
}
