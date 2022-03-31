using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMcontroller : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] firstInfoArr;
    public AudioClip[] secInfoArr;
    public AudioClip intro;
    public AudioClip secInfo;
    public AudioClip question;
    public AudioClip BGM;
    public AudioClip TimeLimit;
    public AudioClip TimeOut;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine(PlayIntro());
    }

    public void PlayBGMByTypes(string Type)
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        
        if (Type == "INTRO")
        {
            audioSource.clip = intro;
            audioSource.loop = false;
        }

        else if (Type == "SecInfo")
        {
            audioSource.clip = secInfo;
            audioSource.loop = false;
        }
        
        else if (Type == "Question")
        {
            audioSource.clip = question;
            audioSource.loop = false;
        }
        
        else if (Type == "BGM")
        {
            audioSource.clip = BGM;
            audioSource.loop = true;
        }
        else if (Type == "LIMIT")
        {
            audioSource.clip = TimeLimit;
            audioSource.loop = false;
        }
        else if (Type == "OUT")
        {
            audioSource.clip = TimeOut;
            audioSource.loop = false;
        }

        audioSource.Play();
    }

    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2f);
        PlayBGMByTypes("INTRO");
    }

    public IEnumerator PlaySecInfo()
    {
        yield return new WaitForSeconds(2f);
        PlayBGMByTypes("SecInfo");
        yield return new WaitForSeconds(13.14f);
        PlayBGMByTypes("BGM");
    }
}
