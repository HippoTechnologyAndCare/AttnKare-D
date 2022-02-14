using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public static float speed = 20f;
    public static Vector3 direction = new Vector3(0,0,1);

    private void OnEnable()                     { StageManager.stage += SetConveyorSpeed; }
    private void OnDisable()                    { StageManager.stage -= SetConveyorSpeed; }
    private void OnTriggerStay(Collider other)  { other.gameObject.GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime; }
    public void SetConveyorSpeed(int stage)     { if (stage > 2) speed = speed * 2; }
}
