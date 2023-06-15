using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    private bool canScale = true;
    private float scaleSpeed = 1.0f;

    // scale presets
    private float scaleLarge = 1.2f;
    private float scaleSmall = 1;
    private float scaleNext;
    private Vector3 scaleOriginal;

    void Start()
    {
        scaleOriginal = transform.localScale;
        scaleNext = scaleLarge;
    }

    void FixedUpdate()
    {
       // UpdateScale();
    }

    internal virtual void UpdateScale()
    {
        if (canScale == false)
            return;

        transform.localScale = Vector3.MoveTowards(transform.localScale, scaleOriginal * scaleNext, scaleSpeed * Time.deltaTime);

        if (transform.localScale * scaleNext == scaleOriginal * scaleLarge)
            scaleNext = scaleSmall;
        if (transform.localScale * scaleNext == scaleOriginal * scaleSmall)
            scaleNext = scaleLarge;
    }

    internal void StopScaling()
    {
        //canScale = false;
        //transform.localScale = scaleOriginal;
    }
}
