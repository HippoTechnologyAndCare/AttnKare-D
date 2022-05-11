using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start!");
        for(int i=0; i<100000; i++)
        {
            Debug.Log("play for moon");
        }
        //UnityMainThreadDispatcher.Enqueue();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("start!");
        for (int i = 0; i < 100000; i++)
        {
            Debug.Log("play for moon");
        }
    }
}
