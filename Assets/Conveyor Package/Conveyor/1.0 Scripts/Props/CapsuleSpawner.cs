using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleSpawner : MonoBehaviour
{
    ObjectPooler m_objectPooler;

    public float m_spawnRate;
    int m_frame = 0;
    int m_index = 0;
    private void Start()
    {
        m_spawnRate = .15f;
        m_objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        m_frame++;
        m_index++;  m_index %= 4;
        if(m_frame > m_spawnRate * 72)
        {
            if (m_index == 0) m_objectPooler.SpawnFromPool("Spawnable", transform.position, Quaternion.identity);
            if (m_index == 1) m_objectPooler.SpawnFromPool("Spawnable1", transform.position, Quaternion.identity);
            if (m_index == 2) m_objectPooler.SpawnFromPool("Spawnable2", transform.position, Quaternion.identity);
            if (m_index == 3) m_objectPooler.SpawnFromPool("Spawnable3", transform.position, Quaternion.identity);

            m_frame = 0;
        }
    }
}
