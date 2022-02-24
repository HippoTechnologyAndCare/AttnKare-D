using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnerType
{
    PlainBoxSpawner,
    OpenBoxSpawner,
    ClosedBoxSpawner,
    ClosedBoxDestroyer
}
public class BoxSpawner : MonoBehaviour
{
    [SerializeField] SpawnerType m_spawnerType;

    [SerializeField] FactoryManager m_factoryManager;                           // Factory Manager Object
    [SerializeField] StageManager m_stageManager;                               // Stage Manager Object

    [SerializeField] GameObject m_boxPrefab;
    [SerializeField] Transform m_spawnPoint;
    [SerializeField] Transform m_spawnables;

    private void OnEnable() { if(m_spawnerType == SpawnerType.PlainBoxSpawner) StageManager.stage += SpawnBox; }
    private void OnDisable() { if (m_spawnerType == SpawnerType.PlainBoxSpawner) StageManager.stage -= SpawnBox; }

    /*public void SpawnBox(int stage) 
    {
        if (stage == 1)      StartCoroutine(SpawnLoop(5, 20)); 
        else if (stage == 2) StartCoroutine(SpawnLoop(10, 20));
        else if (stage == 3) StartCoroutine(SpawnLoop(10, 10));
    }*/

    /*public IEnumerator SpawnLoop(int iterations, float waitTime)
    {

        Instantiate(m_boxPrefab, m_spawnPoint.position, Quaternion.Euler(m_spawnPoint.eulerAngles.x, m_spawnPoint.eulerAngles.y, m_spawnPoint.eulerAngles.z), m_spawnables);
        for (int i = 0; i < iterations - 1; i++)
        {
            yield return new WaitForSeconds(waitTime);
            Instantiate(m_boxPrefab, m_spawnPoint.position, Quaternion.Euler(m_spawnPoint.eulerAngles.x, m_spawnPoint.eulerAngles.y, m_spawnPoint.eulerAngles.z), m_spawnables);
        }
    }*/
    // Erase iterations parameter, make waitTime formula for stage, check currentStage and delegate invoke execution order

    public void SpawnBox(int stage)
    {
        float waitTime = 400 / (20 + (StageManager.m_currentStage - 1) * 5);
        
        for(int i = 0; i < 5; i++)
        {
            Invoke("Spawn", waitTime * i);
        }
    }
    public void Spawn() { Instantiate(m_boxPrefab, m_spawnPoint.position, Quaternion.Euler(m_spawnPoint.eulerAngles.x, m_spawnPoint.eulerAngles.y, m_spawnPoint.eulerAngles.z), m_spawnables); }

    public void SpawnNextBox()
    {
        Instantiate(m_boxPrefab, m_spawnPoint.position, Quaternion.Euler(m_spawnPoint.eulerAngles.x, m_spawnPoint.eulerAngles.y, m_spawnPoint.eulerAngles.z), m_spawnables);
    }
}
