using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text DebuggerUI;

    [SerializeField] Text MainUIText;
    [SerializeField] Image MainUIImage;
    [SerializeField] List<Sprite> m_robotIcons;

    static Text s_MainUIText;
    static Image s_MainUIImage;
    static List<Sprite> s_robotIcons;

    private void Start()
    {
        s_MainUIText = MainUIText;
        s_MainUIImage = MainUIImage;
        s_robotIcons = m_robotIcons;

        SetMainUIImage(0);
    }

    public static void SetMainUIText(int generatedColor)
    {
        // Will Change to Image Later
        switch(generatedColor)
        {
            case 0: s_MainUIText.text = "Yellow"; break;
            case 1: s_MainUIText.text = "Blue"; break;
            case 2: s_MainUIText.text = "Green"; break;
            default: s_MainUIText.text = "-1"; break;
        }
    }
    public static void SetMainUIImage(int index) { s_MainUIImage.sprite = s_robotIcons[index]; }
    public static void EnableMainUIImage() { s_MainUIImage.gameObject.SetActive(true); }
    public static void DisableMainUIImage() { s_MainUIImage.gameObject.SetActive(false); }

    public static void BlinkImage()
    {
        if (!s_MainUIImage.gameObject.activeSelf) EnableMainUIImage();
        s_MainUIImage.enabled = !s_MainUIImage.enabled;
    }

    public static void ResetText(Text uiText) { uiText.text = ""; }
    public static void AddBoxDebuggerText(Text uiText, string text) { uiText.text += text + "\n"; }
    public static void AddBoxDebuggerText(Text uiText, string name, int score)  { uiText.text += name + ": " + score.ToString() + "\n"; }
}
