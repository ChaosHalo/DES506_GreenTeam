using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class SpeedMeasurementPoint : MonoBehaviour
{
    private SpeedMeasurementSystemsTest SpeedMeasurementSystemsTest;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        SpeedMeasurementSystemsTest = GetComponentInParent<SpeedMeasurementSystemsTest>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            try
            {
                SolidController s = other.GetComponent<SolidController>();
                CarManager carManager = other.GetComponent<CarManager>();

                SpeedMeasurementSystemsTest.Info info = new();
                info.Name = carManager.CarInfo.Name;
                info.Speed = s.NormalizedSpeed;
                info.position = other.ClosestPointOnBounds(transform.position);
                info.Time = Time.time;
                SpeedMeasurementSystemsTest.Infos.Add(info);
            }
            catch
            {
                
            }
        }
    }
}
