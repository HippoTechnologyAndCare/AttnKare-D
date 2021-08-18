using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMcontroller : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip Intro;
    public AudioClip BGM;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine(PlayIntro());
    }

    public void PlayBGMByTypes(string Type)
    {
        if (audioSource.clip != null || audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        
        if (Type == "INTRO")
        {
            audioSource.clip = Intro;
        }
        else if (Type == "BGM")
        {
            audioSource.clip = BGM;
        }

        audioSource.Play();
    }

    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(3f);
        PlayBGMByTypes("INTRO");
    }
}
