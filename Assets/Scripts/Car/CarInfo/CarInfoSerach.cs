using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CarInfoSerach : Singleton<CarInfoSerach>, ICarInfoSearch
{
    public List<CarManager> carManagers = new();

    public void SetupCarManagers()
    {
        carManagers.Clear();
        carManagers.AddRange(FindObjectsOfType<CarManager>());
    }

    private void Start()
    {

    }
    /// <summary>
    /// 按位次查询
    /// </summary>
    /// <param name="place"></param>
    /// <returns></returns>
    public CarManager GetCarManager(int place, int lapIndex)
    {
        foreach (var i in carManagers)
        {
            if (i.RankList.Count >= lapIndex && i.RankList[lapIndex - 1] == place)
            {
                return i;
            }
        }
        Debug.LogError("未按照位次查询到车辆信息");
        return null;
    }
    /// <summary>
    /// 按名字查询
    /// </summary>
    /// <param name="carName"></param>
    /// <returns></returns>
    public CarManager GetCarManager(string carName)
    {
        foreach (var i in carManagers)
        {
            if (i.CarInfo.Name == carName)
            {
                return i;
            }
        }
        return null;
    }

    public float GetGapTime(int firstPlace, int nextPlace, int lapIndex)
    {
        CarManager first = GetCarManager(firstPlace, lapIndex);
        CarManager next = GetCarManager(firstPlace, lapIndex);
        if (first == null || next == null)
        {
            throw new Exception("CarManager not found.");
        }
        return first.TimeForOneLapList[lapIndex - 1] - next.TimeForOneLapList[lapIndex - 1];
    }

    public float GetGapTime(string firstCarName, string nextCarName, int lapIndex)
    {
        CarManager first = GetCarManager(firstCarName);
        CarManager next = GetCarManager(nextCarName);
        if (first == null || next == null)
        {
            throw new Exception("CarManager not found.");
        }
        return first.TimeForOneLapList.Last() - next.TimeForOneLapList.Last();
    }

    public float GetLapTime(string carName, int lapIndex)
    {
        CarManager car = GetCarManager(carName);
        if (car == null)
        {
            throw new Exception("CarManager not found.");
        }
        return car.TimeForOneLapList[lapIndex - 1];
    }

    public int GetRank(string carName, int lapIndex)
    {
        CarManager car = GetCarManager(carName);
        try
        {
            return car.RankList.Last();
        }
        catch
        {
            throw new Exception("CarManager not found.");
        }
    }

    public bool IsFinishAfter(string firstCarName, string nextCarName, int lapIndex)
    {
        CarManager first = GetCarManager(firstCarName);
        CarManager next = GetCarManager(nextCarName);
        if (first == null || next == null)
        {
            throw new Exception("CarManager not found.");
        }
        int firstPlace = first.RankList.Last();
        int nextPlace = next.RankList.Last();
        return firstPlace > nextPlace;
    }
}
