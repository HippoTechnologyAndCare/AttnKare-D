using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReAct : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject frame;

    void Start()
    {
        player = GameObject.Find("PlayerController");
        frame = transform.Find("Frame").gameObject;
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y,
                                                player.transform.position.z);
        transform.LookAt(targetPosition);
    }

    IEnumerator ReAct01()
    {
        frame.SetActive(true);
        yield return new WaitForSeconds(2f);
        frame.SetActive(false);
    }
    
}
