using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoButton : MonoBehaviour
{
    // Please, name correctly your variables.
    // Yourself, in 3 months will be grateful    
    private RectTransform rectTransform;
    public RectTransform target;
    public float speed = 1.0f;
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();

    }

    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        rectTransform.position = Vector3.MoveTowards(rectTransform.position, target.position, step);
    }
}
