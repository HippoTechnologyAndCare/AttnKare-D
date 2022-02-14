using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawnerConveyor : MonoBehaviour
{
    [SerializeField] Transform m_spawnedToys;
    [SerializeField] ToySpawner m_toySpawner;

    public float speed = 100f;
    public Vector3 direction = new Vector3(-1, 0, 0);
    private void OnTriggerStay(Collider other) { other.gameObject.GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime; }

    private void OnTriggerExit(Collider other)
    {
        Vector3 _direction = new Vector3(0, 0, -1);
        other.gameObject.GetComponent<Rigidbody>().velocity = speed * _direction * Time.deltaTime;
    }
}
