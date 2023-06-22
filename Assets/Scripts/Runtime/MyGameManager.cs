using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏主流程
/// </summary>
public class MyGameManager : Singleton<MyGameManager>
{
    public GameObject CarPrefab;
    public List<CarInfoScriptableObject> CarConfigList;
    private List<Transform> startPoints;
    // 当前车辆名次
    public int Rank = 1;
    // 车辆总数
    public int CarTotalNum;
    // Start is called before the first frame update
    void Start()
    {
        InitCars();
    }
    
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 初始化车辆
    /// </summary>
    public void InitCars()
    {
        CarTotalNum = CarConfigList.Count;
        Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);
        foreach (var carInfoScriptableObject in CarConfigList)
        {
            GameObject car = Instantiate(CarPrefab, carInfoScriptableObject.CarInfo.StartPoint.transform.position, rotation);
            car.GetComponent<CarManager>().CarInfoScriptableObject = carInfoScriptableObject;

        }
    }
    /// <summary>
    /// 获取排名
    /// </summary>
    /// <returns></returns>
    public int GetRank(bool update = false)
    {
        int res = Rank;
        if (update)
        {
            Rank++;
            if (Rank > CarTotalNum) Rank = 1;
        }

        return res;
    }
}