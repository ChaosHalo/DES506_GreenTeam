using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutterObject : MonoBehaviour
{
    bool showMesh = true;

    GameObject meshObject;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RaceTrackSurface"))
        {
            showMesh = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RaceTrackSurface"))
        {
            showMesh = true;
        }
    }

    private void Start()
    {
        meshObject=transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        showMesh = true;
    }

    private void Update()
    {
        meshObject.SetActive(showMesh);
    }
}
