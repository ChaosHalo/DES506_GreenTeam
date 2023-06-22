using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public CarInfo CarInfo;
    public CarInfoScriptableObject CarInfoScriptableObject;
    public List<int> RankList = new();
    public List<float> TimeForOneLapList = new();
    private float oneLapTime;
    private float totalTime;
    private ArcadeAiVehicleController arcadeAiVehicleController;
    private void Start()
    {
        CarInfo = CarInfoScriptableObject.Init();
        arcadeAiVehicleController = GetComponent<ArcadeAiVehicleController>();
        InitData();
    }
    private void Update()
    {
        Timer();
    }
    private void InitData()
    {
        arcadeAiVehicleController.MaxSpeed = CarInfo.TopSpeed;
        arcadeAiVehicleController.accelaration = CarInfo.Acceleration;
    }
    /// <summary>
    /// 计时器
    /// </summary>
    private void Timer()
    {
        oneLapTime += Time.deltaTime;
        totalTime += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 一圈结束
        if (other.CompareTag("EndLine"))
        {
            OnOneLapEnd();
        }
    }
    /// <summary>
    /// 一圈结束事件
    /// </summary>
    private void OnOneLapEnd()
    {
        TimeForOneLapList.Add(Mathf.Round(oneLapTime * 1000) / 1000f);
        RankList.Add(MyGameManager.instance.GetRank(true));
        oneLapTime = 0;
    }
    /// <summary>
    /// 运行完成事件
    /// </summary>
    public void OnEnd()
    {

    }
    /// <summary>
    /// 运行开始事件
    /// </summary>
    public void OnStart()
    {

    }
}
