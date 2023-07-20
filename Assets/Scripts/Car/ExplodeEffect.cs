using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyParticle", 2.5f);
    }

    private void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
