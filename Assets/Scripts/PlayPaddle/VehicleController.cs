using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VehicleController : MonoBehaviour
{

    //Animator Anim;


    bool moving = false;
    public Vector3 EndPos;

    public TextMeshProUGUI DistanceShow;

    bool GottaGo = false;
    float MovingTimer = 0;
    float MovingTimerForLimit = 2;

    public float Distance = 0;

    void Start()
    {
        MovingTimer = 0;
        //Anim = GetComponent<Animator>();
    }

    void Update()
    {
/*
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Anim.SetFloat("AnimSpeed", 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Anim.SetFloat("AnimSpeed", 1);
        }
*/
        if (GottaGo)
        {
            MovingTimer += Time.deltaTime;

            if (MovingTimer > 1)
            {
                MovingTimer = 0;
                MovingTimerForLimit -= 1;

                if (MovingTimerForLimit == 0)
                {
                    GottaGo = false;
                    moving = false;
                    MovingTimerForLimit = 0;
                }
                else
                {
                    Distance += 2;
                    DistanceShow.text = Distance.ToString();
                }
            }

            if (moving)
            {
                transform.position = Vector3.MoveTowards(transform.position, EndPos, .0042f);
            }
        }
    }

    public void PlusDistance()
    {
        if (!GottaGo)
        {
            GottaGo = true;
            moving = true;
        }

        MovingTimerForLimit = 2;
    }

    public void GameFinish()
    {
        GottaGo = false;
        MovingTimerForLimit = 0;
    }

}
