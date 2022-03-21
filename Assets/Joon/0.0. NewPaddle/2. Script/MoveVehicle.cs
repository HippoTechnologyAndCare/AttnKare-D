using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveVehicle : MonoBehaviour
{
    // Start is called before the first frame update
    bool moving = false;
    public Vector3 EndPos;

    public TextMeshProUGUI DistanceShow;

    bool GottaGo = false;
    float MovingTimer = 0;
    float MovingTimerForLimit = 3;

    public float Distance = 0;
    void Start()
    {
        MovingTimer = 0;
    }

    void Update()
    {
        if (GottaGo)
        {
            MovingTimer += Time.deltaTime;

            if (MovingTimer > 1)
            {
                MovingTimer = 0;
                MovingTimerForLimit -= 1;

                if (MovingTimerForLimit <= 0)
                {
                    GottaGo = false;
                    moving = false;
                    MovingTimerForLimit = 0;
                }
            }

            if (moving)
            {
                transform.position = Vector3.MoveTowards(transform.position, EndPos, .00038f);
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

        MovingTimerForLimit = (25.23f * Manager_Paddle.SDB[Manager_Paddle.intStage].intPercent)/100;
        Debug.Log(MovingTimerForLimit);
    }

    public void GameFinish()
    {
        GottaGo = false;
        MovingTimerForLimit = 0;
    }
}
