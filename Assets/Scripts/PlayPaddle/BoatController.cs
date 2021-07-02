using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    Animation BoatAnim;

    public GameObject Effect_EngineFire;
    public Transform DistanceSlider;

    bool GottaGo = false;
    float EngineTimer = 0;
    float EngineTimerForLimit = 5;

    public float Distance = 0;                 //가야 할 거리 총 100까지


    void Start()
    {
        BoatAnim = GetComponent<Animation>();
        UpdateBoatAnim("IDEL");
    }

    void Update()
    {
        if (GottaGo)
        {
            EngineTimer += Time.deltaTime;

            if (EngineTimer > 1)
            {
                EngineTimer = 0;
                EngineTimerForLimit -= 1;
                Distance += 2f;

                DistanceSlider.GetComponent<Slider>().value = Distance;

                if (EngineTimerForLimit == 0)
                {
                    GottaGo = false;
                    UpdateBoatAnim("PAUSE");
                    Effect_EngineFire.SetActive(false);
                }
            }
        }
    }

    void UpdateBoatAnim(string Status)
    {

        Debug.Log("BOAT ANIM CALL_" + Status);

        if (Status == "IDEL")
        {
            BoatAnim.Play("Boat_Idel");
        }
        else if (Status == "RUN")
        {
            BoatAnim.Play("Boat_Running");
        }
        else if (Status == "PAUSE")
        {
            BoatAnim.Play("Boat_Pause");
        }
        else
        {
            BoatAnim.Play("Boat_Idel");
        }
    }

    public void PlusBoatDistance()
    {
        if (!GottaGo)
        {
            GottaGo = true;
            UpdateBoatAnim("RUN");
            Effect_EngineFire.SetActive(true);
        }

        EngineTimerForLimit = 5;
    }

    public void ResetBoat()
    {
        UpdateBoatAnim("IDEL");
        Distance = 0;
        DistanceSlider.GetComponent<Slider>().value = 0;
    }

    public void GameFinish()
    {
        GottaGo = false;
        EngineTimerForLimit = 5;
        Effect_EngineFire.SetActive(false);
        UpdateBoatAnim("IDEL");
    }

}
