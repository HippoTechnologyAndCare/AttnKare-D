using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPlayBGM : MonoBehaviour
{
    [SerializeField] GameObject soundEffects;
    [SerializeField] GameObject audioTrigger;
    AudioClip intro;
    AudioClip stage1Audio;
    AudioClip stage2Audio;
    AudioClip stage3Audio;
    
    // Start is called before the first frame update
    void Start()
    {
        intro = audioTrigger.GetComponent<AudioSource>().clip;
        stage1Audio = soundEffects.GetComponent<EasyTubeScoreboard>().stage1Audio;
        stage2Audio = soundEffects.GetComponent<EasyTubeScoreboard>().stage2Audio;
        stage3Audio = soundEffects.GetComponent<EasyTubeScoreboard>().stage3Audio;
    }

    // Update is called once per frame
    void Update()
    {
        if (soundEffects.GetComponent<AudioSource>().isPlaying || audioTrigger.GetComponent<AudioSource>().isPlaying)
        {
            if (audioTrigger.GetComponent<AudioSource>().clip == intro || soundEffects.GetComponent<AudioSource>().clip == stage1Audio ||
                soundEffects.GetComponent<AudioSource>().clip == stage2Audio || soundEffects.GetComponent<AudioSource>().clip == stage3Audio)
            {
                StartCoroutine(DecVolume());
            }
        }
        else
        {
            StartCoroutine(IncVolume());
        }
    }

    IEnumerator IncVolume()
    {
        while (GetComponent<AudioSource>().volume < 0.4f)
        {
            GetComponent<AudioSource>().volume += 0.0003f;
            yield return new WaitForSeconds(0.001f);            
        }

        yield break;
    }

    IEnumerator DecVolume()
    {
        while (GetComponent<AudioSource>().volume > 0.1f)
        {
            GetComponent<AudioSource>().volume -= 0.0003f;
            yield return new WaitForSeconds(0.001f);            
        }

        yield break;
    }
}
