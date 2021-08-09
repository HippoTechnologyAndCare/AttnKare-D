using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    Vector3 scoopPos;
    Vector3 scoopRot;
    float timer;
    public GameObject errorMessage;
    public int scoopLostCount = 0;
    
    [Tooltip("Center Camera of XR Rig")]
    public Transform headCamera;

    // Start is called before the first frame update
    void Start()
    {
        scoopPos = gameObject.transform.localPosition;
        scoopRot = gameObject.transform.eulerAngles;
        errorMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If Scoop escapes room, return it to its original position (based on distance between camera and scoop)
        timer += Time.deltaTime;

        if (timer > 2)
        {
            if (Vector3.Distance(gameObject.transform.position, headCamera.position) > 150f)
            {
                Debug.Log("Scoop Lost");
                resetScoop();
            }
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If ball hits boundary outside room, return it to its original position (Only when object escapes room due to extreme force applied)
        if(collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "Terrain")
        {
            Debug.Log("Scoop Hit Boundary");
            resetScoop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Error")
        {
            errorMessage.SetActive(true);
            Debug.Log("Error");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Error")
        {
            errorMessage.SetActive(true);
            Debug.Log("Error");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Error")
        {
            errorMessage.SetActive(false);
            Debug.Log("Out");
        }
    }
    private void resetScoop()
    {
        gameObject.transform.localPosition = scoopPos;
        gameObject.transform.eulerAngles = scoopRot;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        scoopLostCount++;
    }
}
