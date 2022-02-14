using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyCapsule : MonoBehaviour, IPooledObject
{
    public float m_upForce = 1f;
    public float m_sideForce = .1f;
    
    // Custom Start Function
    public void OnObjectSpawn()
    {
        float xForce = Random.Range(-m_sideForce, m_sideForce);
        float yForce = Random.Range(m_upForce / 2f, m_upForce);
        float zForce = Random.Range(-m_sideForce, m_sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }

    private void OnCollisionEnter(Collision collision) { if (collision.gameObject.tag == "Floor") Destroy(gameObject); }
}
