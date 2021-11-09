using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumCard : MonoBehaviour
{
    public string cardNum;
   public TextMeshProUGUI cardText;
    Vector3 initPosition;
    Vector3 initRotation;
 // Transform _lastCollision;
 // Transform collision;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        initRotation = transform.localEulerAngles;


    }

    public void SetCardNum()
    {
        cardText = transform.GetComponentInChildren<TextMeshProUGUI>();
        cardText.text = cardNum;

    }

    public void SetPosRot(Transform posTrigger)
    {
        
        transform.position = posTrigger.position;
        //transform.localEulerAngles = new Vector3(0, 180, 0);
        transform.localEulerAngles = new Vector3(90, 180, 0);

    }

    public void ResetPosRot()
    {
        /*
        if (transform.GetComponent<Rigidbody>().useGravity)
        {
            transform.GetComponent<Rigidbody>().useGravity = false;
        }
        */
        transform.position = initPosition;
        transform.localEulerAngles = initRotation;



    }

    public void SetGravity()
    {
    }
    /*
    public void DisableCollision(Transform colObject)
    {
        
        Debug.Log("diabled");
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true );
        if (_lastCollision != null && _lastCollision != colObject ) 
        {
            //  _lastCollision.GetComponent<Collider>().isTrigger = false;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true);
            _lastCollision = colObject;
        //  colObject.GetComponent<Collider>().isTrigger = true;
                
        }
        if(_lastCollision == null)
        {
            _lastCollision = colObject;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true);

        }
        else
        {
            return;
        }
        
       

    }

    public void EnableCollision(Transform colObject)
    {
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), false);

    }

    */






}
