using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public Text DebuggerUI;

    // Main UI Components
    [SerializeField] RectTransform MainUIRect;
    [SerializeField] Text MainUIText;
    [SerializeField] Image MainUIImage;
    [SerializeField] List<Sprite> m_robotIcons;
    [SerializeField] List<string> AudioLine;

    // Skip Canvas Components
    [SerializeField] GameObject m_skipCanvas;
    [SerializeField] Text m_skipCanvasText;

    // Static Member Variables
    public static RectTransform s_MainUIRect;
    public static Text s_MainUIText;
    public static Image s_MainUIImage;
    public static GameObject s_TextUIObject;
    public static Text s_TextUIText;
    public static List<string> s_AudioLine;
    public static List<Sprite> s_robotIcons;
    public static GameObject s_skipCanvas;
    public static Text s_skipCanvasText;

    void Awake() => instance = this;

    private void Start()
    {
        s_MainUIRect = MainUIRect;
        s_MainUIText = MainUIText;
        s_MainUIImage = MainUIImage;

        s_AudioLine = new List<string>();
        for (int i = 0; i < AudioLine.Count; i++) s_AudioLine.Add(AudioLine[i]);

        s_robotIcons = m_robotIcons;

        s_skipCanvas = m_skipCanvas;
        s_skipCanvasText = m_skipCanvasText;

        SetMainUIImage(0);
    }

    public static void SetUIText(Text textCmp, string text) => textCmp.text = text;

    #region MAIN UI FUNCTIONS
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

    public static void ScaleUI()
    {
        instance.StartCoroutine(instance.ScaleUIcr());
    }

    IEnumerator ScaleUIcr()
    {
        float height = s_MainUIRect.rect.height;

        while(s_MainUIRect.rect.height >= 900)
        {
            height -= 4;
            s_MainUIRect.sizeDelta = new Vector2(s_MainUIRect.rect.width, height);
            yield return new WaitForSeconds(.001f);
        }

        yield break;
    }
    public static void EnableMainUIImage()
    {
        s_MainUIImage.gameObject.SetActive(true);
        if (!s_MainUIImage.enabled) s_MainUIImage.enabled = true;
    }
    public static void DisableMainUIImage() { s_MainUIImage.gameObject.SetActive(false); }

    public static void BlinkImage()
    {
        if (!s_MainUIImage.gameObject.activeSelf) EnableMainUIImage();
        s_MainUIImage.enabled = !s_MainUIImage.enabled;
    }
    #endregion

    #region SKIP CANVAS FUNCTIONS
    public static void ShowSkipCanvas() { s_skipCanvas.SetActive(true); }
    public static void HideSkipCanvas() { s_skipCanvas.SetActive(false); }
    public static IEnumerator CountSecondsOnCanvas()
    {
        s_skipCanvasText.text = "3";
        yield return new WaitForSeconds(1f);

        s_skipCanvasText.text = "2";
        yield return new WaitForSeconds(1f);

        s_skipCanvasText.text = "1";
        yield return new WaitForSeconds(1f);

        FactoryManager.LoadNextScene();
    }
    #endregion

    #region DEBUGGER UI FUNCTIONS
    public static void ResetText(Text uiText) { uiText.text = ""; }
    public static void AddBoxDebuggerText(Text uiText, string text) { uiText.text += text + "\n"; }
    public static void AddBoxDebuggerText(Text uiText, string name, int score)  { uiText.text += name + ": " + score.ToString() + "\n"; }
    #endregion
}
