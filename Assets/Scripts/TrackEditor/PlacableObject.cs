using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class PlacableObject : MonoBehaviour
{
    [SerializeField]
    internal InputManager inputManager;
    [SerializeField]
    internal AutoRotate autoRotate;

    [SerializeField]
    internal bool canScale = true;
    [SerializeField]
    private float scaleSpeed = 0.01f;

    // scale presets
    private float multiLarge = 1.2f;
    private float multiSmall = 1.0f;
    private Vector3 scaleSmall;
    private Vector3 scaleLarge;
    private Vector3 scaleNext;
    private Vector3 scaleOriginal;

    private void Awake()
    {
        scaleOriginal = transform.parent.localScale;
        scaleSmall = scaleOriginal * multiSmall;
        scaleSmall.y = scaleOriginal.y;
        scaleLarge = scaleOriginal * multiLarge;
        scaleLarge.y = scaleOriginal.y;
        scaleNext = scaleLarge;

        inputManager = FindObjectOfType<InputManager>();
    }

    private void FixedUpdate()
    {
        UpdateScale();
    }

    internal virtual void UpdateScale()
    {
        if (canScale == false)
            return;

        transform.parent.localScale = Vector3.MoveTowards(transform.parent.localScale, scaleNext, scaleSpeed * Time.deltaTime);

        if (transform.parent.localScale == scaleLarge)
            scaleNext = scaleSmall;
        if (transform.parent.localScale == scaleSmall)
            scaleNext = scaleLarge;
    }

    internal void StopScaling()
    {
        canScale = false;
        transform.parent.localScale = scaleOriginal;
    }
}
