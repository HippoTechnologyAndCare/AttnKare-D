using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCapsule : MonoBehaviour
{
    [SerializeField] GameObject capsuleClone;
    [SerializeField] GameObject capsuleClone1;
    [SerializeField] GameObject capsuleClone2;
    [SerializeField] GameObject capsuleClone3;
    [SerializeField] Transform spawnPoint;
    public bool isSpawning;
    public int spawnRate;
    int frame = 0;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnRate = 18;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if(frame > spawnRate * 72 && isSpawning)
        {
            index = Random.Range(0, 3);
            if (index == 0) Instantiate(capsuleClone,  spawnPoint.position, Quaternion.Euler(180, 0, 0));
            if (index == 1) Instantiate(capsuleClone1, spawnPoint.position, Quaternion.Euler(180, 0, 0));
            if (index == 2) Instantiate(capsuleClone2, spawnPoint.position, Quaternion.Euler(180, 0, 0));
            if (index == 3) Instantiate(capsuleClone3, spawnPoint.position, Quaternion.Euler(180, 0, 0));

            frame = 0;
        }
    }
}
