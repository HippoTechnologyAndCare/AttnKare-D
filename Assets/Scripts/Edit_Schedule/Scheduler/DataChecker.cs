using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class ScheduleData
{
    public float data210;
    public float data211;
    public float data212;
}

namespace Scheduler
{
    public class DataChecker : MonoBehaviour
    {
        public ScheduleData scheduleData = new ScheduleData();
        [SerializeField] private ScoreManager02 scoreManager;

        public void IncreaseD210()
        {
            scheduleData.data210 += 1;
        }

        public void SetScoreD211()
        {
            scheduleData.data211 = scoreManager.scoreI;
        }
        
        public void SetScoreD212()
        {
            scheduleData.data212 = scoreManager.scoreI;
        }
    }
}


