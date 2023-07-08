using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCar : MonoBehaviour
{
    public float ForwardForce;
    public float DownForce;
    private Rigidbody rb;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * Time.fixedDeltaTime * ForwardForce);
        rb.AddForce(-transform.up * Time.fixedDeltaTime * DownForce);
    }
}
