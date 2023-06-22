using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CarInfoSerach : Singleton<CarInfoSerach>, ICarInfoSearch
{
    public List<CarManager> _carManagers;

    public List<CarManager> carManagers
    {
        get
        {
            if (_carManagers == null || _carManagers.Count == 0)
            {
                _carManagers = new List<CarManager>(FindObjectsOfType<CarManager>());
            }
            return _carManagers;
        }
    }

    private void Start()
    {

    }
    /// <summary>
    /// 按位次查询
    /// </summary>
    /// <param name="place"></param>
    /// <returns></returns>
    private CarManager GetCarManager(int place, int lapIndex)
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
    private CarManager GetCarManager(string carName)
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
        return first.TimeForOneLapList[lapIndex - 1] - next.TimeForOneLapList[lapIndex - 1];
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
            return car.RankList[lapIndex - 1];
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
        int firstPlace = first.RankList[lapIndex - 1];
        int nextPlace = next.RankList[lapIndex - 1];
        return firstPlace > nextPlace;
    }
}
