using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class CarManager : MonoBehaviour
{
    public CarInfo CarInfo;
    public CarInfoScriptableObject CarInfoScriptableObject;
    public List<int> RankList = new();
    public List<float> TimeForOneLapList = new();
    private float oneLapTime;
    private float totalTime;
    private VehicleInformation vehicleInformation;

    private void Awake()
    {
        InitData();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        Timer();
    }
    public void InitData()
    {
        CarInfo = CarInfoScriptableObject.Init();
        vehicleInformation = GetComponent<VehicleInformation>();
        vehicleInformation.LobbyName = CarInfo.Name;
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
        if (other.CompareTag("Checkpoint"))
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
