using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class AcceleratedPlot : MonoBehaviour
{
    public struct CarData
    {
        public float EngineForce;
        public float Grip;
        public float TopSpeed;
    }
    Dictionary<string, CarData> originCarDatas = new();
    //开始
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            SolidController s = other.GetComponent<SolidController>();
            string carName = s.GetComponent<CarManager>().CarInfo.Name;

            CarData originCarData = new CarData();
            originCarData.TopSpeed = s.FullThrottleVelocity;
            originCarData.EngineForce = s.EngineForce;
            originCarData.Grip = s.CarGrip;

            if (!originCarDatas.ContainsKey(carName))
            {
                originCarDatas.Add(carName, originCarData);
            }

            float k = 2f;
            CarData accCarData = new CarData();
            accCarData.TopSpeed = s.FullThrottleVelocity * k;
            accCarData.EngineForce = s.EngineForce * k;
            accCarData.Grip = s.CarGrip * k;
            SetCarData(accCarData, s);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            SolidController s = other.GetComponent<SolidController>();
            string carName = s.GetComponent<CarManager>().CarInfo.Name;

            if (originCarDatas.ContainsKey(carName))
            {
                SetCarData(originCarDatas[carName], s);
                originCarDatas.Remove(carName);
            }
        }
    }

    private void SetCarData(CarData carData, SolidController s)
    {
        s.FullThrottleVelocity = carData.TopSpeed;
        s.EngineForce = carData.EngineForce;
        s.CarGrip = carData.Grip;
    }
}
