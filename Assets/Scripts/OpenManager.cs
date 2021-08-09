using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

using UnityEngine.Rendering;



public class OpenManager : MonoBehaviour
{

    public GameObject XRRig;
    public GameObject Ghost;
    public Volume global;
    VolumeProfile globalVolume;
    public float bloom = 10f;
    public float color;
    float xrrigZ;
    Vector3 des;
    Vector3 startdes;
    public bool tick;

    ColorAdjustments _coloradjustment = null;



    // Start is called before the first frame update
    void Start()
    {
        des = new Vector3(0.52f, -0.64f, 2.04f);

        MoveGhost();

        
    }

    // Update is called once per frame
    void Update()
    {
        MoveGhost();
        xrrigZ = XRRig.transform.position.z;

    }

    private void LateUpdate()
    {
       
    }

    private void MoveGhost()
    {
        if (tick)
        {
            Ghost.GetComponent<Actor>().SimpleMove(des);
            globalVolume = global.sharedProfile;
            globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

            _coloradjustment.saturation.value = -100f;

        }


        if(!tick)
        {
            Ghost.GetComponent<Actor>().SimpleMove(startdes);

        }
    }


}
