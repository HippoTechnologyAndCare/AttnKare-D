using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class raycast : MonoBehaviour
{
    private RaycastHit hit;
    private float maxDistance = 300f;
    public GameObject originofray;
    public GameObject Score1_canvas;
    private TextMesh Score1_Text;
    int score1 = 0;
    GameObject target = null;
    GameObject prefab_obj;
    Vector3 bubblePosition;
    public AudioSource bubbleSound;
    [SerializeField] TextMeshProUGUI m_Object;

    void Start()
    {
        prefab_obj = Resources.Load("MS/SubEmitterDeath") as GameObject;
        //Score1_Text = Score1_canvas.GetComponent<TextMesh>();
    }

    void Update() {

        if (Physics.Raycast(originofray.transform.position, originofray.transform.forward, out hit, maxDistance))
        {
            //Debug.Log("hit point : " + hit.point + ", distance : " + hit.distance + ", name : " + hit.collider.name);
            //Debug.DrawRay(originofray.transform.position, originofray.transform.forward * hit.distance, Color.red); }
            //Debug.DrawRay(originofray.transform.position, originofray.transform.forward * 1000f, Color.red);
            if (Input.GetButtonDown("XRI_Right_TriggerButton"))
            {
                if (hit.collider.tag == "Bubble")
                {
                    //bubblePosition = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);
                    bubbleSound.Play();
                    Destroy(hit.collider.gameObject);
                    Instantiate(prefab_obj, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z), Quaternion.identity);
                    score1++;
                    m_Object.text = score1.ToString();

                    
                    //Instantiate(prefab_obj, bubblePosition , Quaternion.identity);
                }
            }
        }
        else
        {
            //Debug.DrawRay(originofray.transform.position, originofray.transform.forward * 1000f, Color.red);
        }
        
    }
}

