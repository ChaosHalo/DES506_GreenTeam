using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutterObject : MonoBehaviour
{
    bool isCovered = false;

    public GameObject meshObject;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isCovered = true;
        }
    }

    private void FixedUpdate()
    {
        if(meshObject !=null)
        meshObject.SetActive(!isCovered);
        isCovered = false;
    }
}
