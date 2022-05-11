using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createBubble : MonoBehaviour
{
    public float TimeLeft = 3.5f;
    private float nextTime = 0.0f;
    public GameObject theCreated;
    
    public GameObject theCreated2;
    public GameObject theCreated3;
    public GameObject theCreated4;
    public Transform bubParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextTime)
        {
            nextTime = Time.time + TimeLeft;
            createBubbles();
        }
 
    }
    void createBubbles()
    {
        //Instantiate(theCreated);
        Instantiate(theCreated, new Vector3(0, 2, 0), Quaternion.identity, bubParent);
        Instantiate(theCreated2, new Vector3(0, 2, 0), Quaternion.identity, bubParent);
        Instantiate(theCreated3, new Vector3(0, 2, 0), Quaternion.identity, bubParent);
        Instantiate(theCreated4, new Vector3(0, 2, 0), Quaternion.identity, bubParent);
        //.transform.SetParent(bubParent);
    }
}
