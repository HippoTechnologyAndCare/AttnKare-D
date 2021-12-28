using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextandSpeech : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("NARRATION")]
    public AudioClip[] arrNarr;
    [Header("Warning")]
    public AudioClip[] arrWarning;
    AudioSource audioPlay;

    [Header("TEXT")]
    public GameObject go_SpeechBubble; // canvas or gameobject(turned off when not speaking)
    [SerializeField]
    private TextAsset txta_speech; // speech
    List<string> list_text;
    private TextMeshProUGUI txt_speech; //text in canvas
    [SerializeField]
    private char divider;


    /* Call
     * yield return StartCoroutine(narration.Introduction());
     * where you want to start your introduction
     * 
     */
    private void Start()
    {
        audioPlay = GetComponent<AudioSource>();
        txt_speech = go_SpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
    }

    private List<string> TextToList(TextAsset txta_speech)
    {
        var listToReturn = new List<string>();
        var arrayString = txta_speech.text.Split(divider);
        foreach (var line in arrayString)
        {
            listToReturn.Add(line);
        }
        return listToReturn;
    }
    private void NarrPlay(AudioClip nowclip)
    {
        audioPlay.clip = nowclip;
        audioPlay.Play();
    }

    public IEnumerator Introduction()
    {
        list_text = TextToList(txta_speech);
        for (int i = 0; i < list_text.Count; i++)
        {
            go_SpeechBubble.SetActive(true);
            NarrPlay(arrNarr[i]);
            txt_speech.text = list_text[i];
            yield return new WaitWhile(() => audioPlay.isPlaying);
            go_SpeechBubble.SetActive(false);
            yield return new WaitForSeconds(0.9f);
        }
        go_SpeechBubble.SetActive(false);

    }
}
