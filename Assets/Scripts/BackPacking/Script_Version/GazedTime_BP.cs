using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazedTime_BP : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject goGazed;
    public Object_BP.GAZE_BP GazedObject;

    float m_fTimetable;
    float m_fTV;
    float m_fMemo;
    private void Update()
    {
        switch (GazedObject)
        {
            case Object_BP.GAZE_BP.MEMO: Memo(); break;
            case Object_BP.GAZE_BP.TV: Television(); break;
            case Object_BP.GAZE_BP.TIMETABLE: TimeTable(); break;
            case Object_BP.GAZE_BP.NOTWATCHING: break;
        }
    }

    private void TimeTable()
    {
        m_fTimetable += Time.deltaTime;

    }

    private void Memo()
    {
        m_fMemo += Time.deltaTime;
    }

    private void Television()
    {
        m_fTV += Time.deltaTime;
    }

    void NotWatching()
    {
        goGazed = null;
    }
}
