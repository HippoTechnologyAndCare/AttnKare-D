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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "HeadCollision")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE END");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.name == "HeadCollision")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE START");

            audioSource.clip = Return;
            audioSource.Play();
        }
    }
}
