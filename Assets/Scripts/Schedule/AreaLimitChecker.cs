using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLimitChecker : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip Return;

    public Transform BehaviorMG;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE END");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE START");

            audioSource.clip = Return;
            audioSource.Play();
        }
    }
}
