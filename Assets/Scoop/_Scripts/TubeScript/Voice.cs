using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    [SerializeField] AudioClip stage1FinishAudio;
    [SerializeField] AudioClip stage2FinishAudio;
    [SerializeField] AudioClip stage3FinishAudio;

    public void Stage1Finish()
    {
        GetComponent<AudioSource>().clip = stage1FinishAudio;
        GetComponent<AudioSource>().Play();
    }

    public void Stage2Finish()
    {
        GetComponent<AudioSource>().clip = stage2FinishAudio;
        GetComponent<AudioSource>().Play();
    }

    public void Stage3Finish()
    {
        GetComponent<AudioSource>().clip = stage3FinishAudio;
        GetComponent<AudioSource>().Play();
    }
}
