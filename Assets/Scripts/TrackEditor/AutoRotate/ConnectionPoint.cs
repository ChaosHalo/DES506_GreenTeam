using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // do raycast
        RaycastHit[] hits = Physics.RaycastAll(transform.position, new Vector3(1, 0, 0), 40);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
