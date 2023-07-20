using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutterObject : MonoBehaviour
{
    bool enableObject = true;

    GameObject meshObject;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RaceTrackSurface"))
        {
            enableObject = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RaceTrackSurface"))
        {
            enableObject = true;
        }
    }

    private void Start()
    {
        meshObject=transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        enableObject = true;
    }

    private void Update()
    {
        meshObject.SetActive(enableObject);
    }
}
