using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VehicleController : MonoBehaviour
{
    Animation Anim;
    float prevSpeed;

    public TextMeshProUGUI DistanceShow;

    bool GottaGo = false;
    float MovingTimer = 0;
    float MovingTimerForLimit = 2;

    public float Distance = 0;


    void Start()
    {
        Anim = GetComponent<Animation>();
        Anim.enabled = false;
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

                if (MovingTimerForLimit > 0)
                {
                    Distance += 1;
                    DistanceShow.text = Distance.ToString();
                }

                if (MovingTimerForLimit == 0)
                {
                    GottaGo = false;
                    Anim.enabled = false;
                    MovingTimerForLimit = 0;
                }
            }
        }
    }

    public void PlusDistance()
    {
        if (!GottaGo)
        {
            GottaGo = true;
            Anim.enabled = true;
        }

        MovingTimerForLimit = 2;
    }

    public void GameFinish()
    {
        GottaGo = false;
        MovingTimerForLimit = 0;
    }

}
