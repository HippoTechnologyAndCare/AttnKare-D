using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] Transform CenterEyeAnchor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MainCamera")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider>().enabled = false;
            CenterEyeAnchor.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
