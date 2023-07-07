using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using MoreMountains.HighroadEngine;
public class CarInfoSerach : Singleton<CarInfoSerach>, ICarInfoSearch
{
    
    private List<BaseController> GetBaseControllers()
    {
        return FindObjectsOfType<BaseController>().ToList();
    }
    private BaseController GetCarController(string name)
    {
        List<BaseController> baseControllers = GetBaseControllers();
        foreach(var i in baseControllers)
        {
            if (i.GetComponent<CarManager>().CarInfo.Name == name) return i;
        }
        return null;
    }
    private BaseController GetCarController(int place)
    {
        List<BaseController> baseControllers = GetBaseControllers();
        foreach (var i in baseControllers)
        {
            if (i.FinalRank == place) return i;
        }
        return null;
    }
    public float GetGapTime(int firstPlace, int nextPlace)
    {
        BaseController first = GetCarController(firstPlace);
        BaseController next = GetCarController(nextPlace);
        if(first == null || next == null) return 0.0f;
        return first.GetComponent<CarManager>().FinalTime - next.GetComponent<CarManager>().FinalTime;
    }

    public float GetGapTime(string firstCarName, string nextCarName)
    {
        BaseController first = GetCarController(firstCarName);
        BaseController next = GetCarController(nextCarName);
        if (first == null || next == null) return 0.0f;
        return first.GetComponent<CarManager>().FinalTime - next.GetComponent<CarManager>().FinalTime;
    }

    public float GetLapTime(string carName)
    {
        BaseController car = GetCarController(carName);
        return car.GetComponent<CarManager>().FinalTime;
    }

    public int GetRank(string carName)
    {
        BaseController car = GetCarController(carName);
        return car.FinalRank;
    }

    public bool IsFinishAfter(string firstCarName, string nextCarName)
    {
        return GetGapTime(firstCarName, nextCarName) > 0.0f;
    }
}
