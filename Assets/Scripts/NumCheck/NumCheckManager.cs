using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class NumCheckManager : MonoBehaviour
{
    public Transform Grabber;
    public GameObject[] arrCards;
   
    Transform colObject;
    Transform _lastCollision;
    Transform collision;

    // Start is called before the first frame update
    void Start()
    {
        int[] arrNum = new int[arrCards.Length];

        for (int i = 0; i < arrCards.Length; i++)
        {
            arrNum[i] = i + 1;
        }

        ShuffleNum(arrNum); //shuffle numbers

        for (int count = 0; count < arrCards.Length; count++)
        {
            int num = arrNum[count];
            string num_s = num.ToString();

            arrCards[count].GetComponent<NumCard>().cardNum = num_s;
            arrCards[count].GetComponent<NumCard>().SetCardNum();
            Debug.Log(arrCards[count].transform.name);

        }

    }

    public int[] ShuffleNum(int[] arrNum)
    {

        for(int i =0; i < arrNum.Length; i++)
        {
            int rnd_n = Random.Range(0, arrNum.Length);
            int temp = arrNum[i];
            arrNum[i] = arrNum[rnd_n];
            arrNum[rnd_n]=temp;


        }
        return arrNum;

    }

    public void DisableCollision(Transform colObject)
    {

        
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true );
        if (_lastCollision != null && _lastCollision != colObject)
        {
            //  _lastCollision.GetComponent<Collider>().isTrigger = false;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true);
            Debug.Log("diabled");
            _lastCollision = colObject;
            //  colObject.GetComponent<Collider>().isTrigger = true;

        }
        if (_lastCollision == null)
        {
            _lastCollision = colObject;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), true);

        }
        else
        {
            return;
        }



    }

    public void EnableCollision()
    {
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("numCard"), LayerMask.NameToLayer("numCard"), false);

    }




}
