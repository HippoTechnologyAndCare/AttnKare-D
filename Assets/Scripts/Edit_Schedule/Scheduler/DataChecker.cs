using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class ScheduleData
{
    public float data210;
}

namespace Scheduler
{
    public class DataChecker : MonoBehaviour
    {
        public ScheduleData scheduleData = new ScheduleData();

        public void IncreaseD210()
        {
            scheduleData.data210 += 1;
        }
    }
}


