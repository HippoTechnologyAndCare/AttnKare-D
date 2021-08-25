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

    [Header("For Some Necessary Variables")]
    [SerializeField] GameObject scoreboard;
    [SerializeField] GameObject audioTrigger;

    bool shovelGrabShown = false;

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
        if (!audioTrigger.GetComponent<AudioSource>().isPlaying && !shovelGrabShown)
        {
            StartCoroutine(ShowMessage(shovelGrab));
            shovelGrabShown = true;
        }

        if (ballPool.activeSelf)
        {
            Destroy(shovelGrab);
        }
    }

    public IEnumerator ShowMessage(GameObject message)
    {
        message.SetActive(true);
        Debug.Log("Show Message");

        yield return new WaitForSeconds(5f);

        message.SetActive(false);
        Debug.Log("Hide Message");
    }
}
