using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    enum SpawnerColor
    {
        Green,
        Blue,
        Yellow
    }

    [SerializeField] SpawnerColor m_spawnerColor;

    ObjectPooler m_objectPooler;

    float m_time = 0;
    int m_spawnCount = 0;
    int m_isActive = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        // Initial Spawn
        if(m_time > .6f && m_isActive < 30)
        {
            SpawnToyFromPool();
            m_isActive++;   m_time = 0;
        }

        // Spawn Rate after All Toys are Active
        if (m_time > 2f && m_spawnCount < 2)
        {
            SpawnToyFromPool();
            m_spawnCount++; m_time = 0;
        }

        if (m_time > 11) m_spawnCount = 0;
    }

    void SpawnToyFromPool()
    {
        if (m_spawnerColor == SpawnerColor.Green)   m_objectPooler.SpawnFromPool("Green",   transform.position, Quaternion.Euler(-90, 0, 0));
        if (m_spawnerColor == SpawnerColor.Blue)    m_objectPooler.SpawnFromPool("Blue",    transform.position, Quaternion.Euler(-90, 0, 0));
        if (m_spawnerColor == SpawnerColor.Yellow)  m_objectPooler.SpawnFromPool("Yellow",  transform.position, Quaternion.Euler(-90, 0, 0));
    }
}
