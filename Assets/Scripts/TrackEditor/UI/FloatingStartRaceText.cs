using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingStartRaceText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private float moveSpeed = 100;
    private float vectorMulti = 1000;
    private Vector3 vectorDirection = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeleteAfterDelay", 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, vectorDirection * vectorMulti, Time.deltaTime * moveSpeed);
    }

    void DeleteAfterDelay()
    {
        Destroy(gameObject);
    }

    internal void SetupVariables(string errorMessage)
    {
        text.text = errorMessage;
    }
}
