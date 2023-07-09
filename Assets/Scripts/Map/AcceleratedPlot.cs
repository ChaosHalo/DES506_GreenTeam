using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class AcceleratedPlot : MonoBehaviour
{
    [System.Serializable]
    public struct CarData
    {
        public float EngineForce;
        public float Grip;
        public float TopSpeed;
    }
    public LoopAccleartion loopAccleartion;
    Dictionary<string, CarData> originCarDatas = new();
    public LayerMask carLayer;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        //carLayer = LayerMask.GetMask(GlobalConstants.CARMASKNAME);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            Acc(other);
            //ForwardDirectly(other);
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
                SetGravity(other.gameObject, true);
                SetCarData(originCarDatas[carName], s);
                originCarDatas.Remove(carName);
                SetCarCollision(false);
            }
        }
    }
    private void ForwardDirectly(Collider other)
    {
        VehicleAI vehicleAI = other.GetComponent<VehicleAI>();
        SolidController s = other.GetComponent<SolidController>();
        vehicleAI.enabled = false;
        s.enabled = false;

        if(other.GetComponent<LoopCar>() == null)
        {
            other.gameObject.AddComponent<LoopCar>();
        }
    }
    private void Acc(Collider other)
    {
        SolidController s = other.GetComponent<SolidController>();
        string carName = s.GetComponent<CarManager>().CarInfo.Name;
        SetCarCollision(true);
        CarData originCarData = new CarData();
        originCarData.TopSpeed = s.FullThrottleVelocity;
        originCarData.EngineForce = s.EngineForce;
        originCarData.Grip = s.CarGrip;

        if (!originCarDatas.ContainsKey(carName))
        {
            originCarDatas.Add(carName, originCarData);
        }

        //float k = 30f;
        CarData accCarData = new CarData();
        accCarData.TopSpeed = loopAccleartion.acceleratedInfo.TopSpeed;
        accCarData.EngineForce = loopAccleartion.acceleratedInfo.EngineForce;
        accCarData.Grip = loopAccleartion.acceleratedInfo.Grip;
        SetGravity(other.gameObject, false);
        Debug.Log("加速模式");
        SetCarData(accCarData, s);
    }
    private void SetCarCollision(bool activeIgnore)
    {
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer(GlobalConstants.CARMASKNAME), LayerMask.NameToLayer(GlobalConstants.CARMASKNAME), activeIgnore);
    }
    private void SetGravity(GameObject gameObject,bool active)
    {
        // gameObject.GetComponent<Rigidbody>().useGravity = active;
        // gameObject.GetComponent<Rigidbody>().mass = active ? 1 : 800;
        //Physics.gravity = active ? new Vector3(0, -29f, 0) : new Vector3(0, -15f, 0);
    }   
    private void SetCarData(CarData carData, SolidController s)
    {
        s.FullThrottleVelocity = carData.TopSpeed;
        s.EngineForce = carData.EngineForce;
        s.CarGrip = carData.Grip;
    }
}
