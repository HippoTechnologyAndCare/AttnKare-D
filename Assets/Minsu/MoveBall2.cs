using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall2 : MonoBehaviour
{
    public float inten = 0.1f;
    float timespan = 0.0f;
    public float checkTime = 3.0f;
    bool uptoward = true;
    int ranX;
    int ranZ;
    public float FloatStrenght;
    public float RandomRotationStrenght;
    Vector3 RandomV;
    void Start()    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RandomV = new Vector3(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f));
        transform.GetComponent<Rigidbody>().AddForce(RandomV * FloatStrenght);
        transform.Rotate(RandomRotationStrenght, RandomRotationStrenght, RandomRotationStrenght);
        /*
        timespan += Time.deltaTime;
        if (uptoward == true)
        {
            transform.Translate(new Vector3(0, 0.01f * inten, 0));
            //Debug.Log("up");
        }
        else
        {
            transform.Translate(new Vector3(0, -0.01f * inten, 0));
            
        }
        if (timespan > checkTime){
            uptoward = !uptoward;
            timespan = 0;
        }
        
        timespan += Time.deltaTime;
        ranX = Random.Range(-5, 6);
        ranZ = Random.Range(-5, 6);
        if (uptoward == true)
        {
            transform.Translate(new Vector3(0.001f * ranX, 0.01f * inten, 0.001f * ranZ));
            //Debug.Log("up");
        }
        else
        {
            transform.Translate(new Vector3(0.001f * ranX, -0.01f * inten, 0.001f * ranZ));

        }
        if (timespan > checkTime)
        {
            uptoward = !uptoward;
            timespan = 0;
        }

        if (Input.GetButtonDown("XRI_Right_TriggerButton"))
        {
            Destroy(gameObject);
        }
        */


    }
}
