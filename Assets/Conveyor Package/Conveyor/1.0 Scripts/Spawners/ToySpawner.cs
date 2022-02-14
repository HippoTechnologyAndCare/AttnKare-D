using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawner : MonoBehaviour
{
    [SerializeField] Transform m_toySpawnPoint;
    [SerializeField] Transform m_spawnedToys;
    [SerializeField] GameObject m_grabbableToy;

    [SerializeField] ToySpawnerConveyor m_toySpawnerConveyor; 

    float m_time = 0;
    int m_spawnCount = 0;

    // Update is called once per frame
    // 4, 20
    void Update()
    {
        m_time += Time.deltaTime;
        
        if(m_time > 2f && m_spawnCount < 2) 
        { 
            Instantiate(m_grabbableToy, m_toySpawnPoint.position, Quaternion.Euler(m_toySpawnPoint.eulerAngles.x, m_toySpawnPoint.eulerAngles.y, m_toySpawnPoint.eulerAngles.z), m_spawnedToys);
            m_spawnCount++;   m_time = 0; 
        }

        if (m_time > 11) m_spawnCount = 0;
    }
}
