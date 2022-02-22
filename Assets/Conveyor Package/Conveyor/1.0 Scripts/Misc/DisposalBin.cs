using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposalBin : MonoBehaviour
{
    List<GameObject> m_disposedToys;

    private void Start() { m_disposedToys = new List<GameObject>(); }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Robot")) m_disposedToys.Add(other.gameObject);
        if (m_disposedToys.Count >= 10) StartCoroutine(EmptyBin());
    }

    private void OnTriggerExit(Collider other) { m_disposedToys.Remove(other.gameObject); }

    // Wait 2 Seconds and Empty Disposal Bin
    IEnumerator EmptyBin() 
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject toy in m_disposedToys) Destroy(toy); 
    }
}
