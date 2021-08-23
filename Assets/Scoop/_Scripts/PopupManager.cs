using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] public GameObject shovelGrab;
    [SerializeField] public GameObject ballPool;
    [SerializeField] public GameObject colorGuide;
    [SerializeField] public GameObject numberGuide;
    public int ballPoolCount = 0;


    [Header("For Some Necessary Variables")]
    [SerializeField] GameObject scoreboard;

    // Start is called before the first frame update
    void Start()
    {
        shovelGrab.SetActive(false);
        ballPool.SetActive(false);
        colorGuide.SetActive(false);
        numberGuide.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!scoreboard.GetComponent<AudioSource>().isPlaying)
        {
            StartCoroutine(ShowMessage(shovelGrab));
        }
    }

    public IEnumerator ShowMessage(GameObject message)
    {
        message.SetActive(true);

        yield return new WaitForSeconds(5f);

        message.SetActive(false);
    }
}
