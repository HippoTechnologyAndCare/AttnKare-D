using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenManager : MonoBehaviour
{

    public GameObject Ghost;
    Vector3 des;
    public bool tick;

    // Start is called before the first frame update
    void Start()
    {
        des = new Vector3(0.52f, -0.64f, 2.04f);

        MoveGhost();
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveGhost();


     
    }

    private void MoveGhost()
    {
        if (tick)
        {
            Ghost.GetComponent<Actor>().SimpleMove(des);
        }
    }
}
