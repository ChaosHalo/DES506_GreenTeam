using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using MoreMountains.HighroadEngine;
public class CarInfoSearch : Singleton<CarInfoSearch>, ICarInfoSearch
{
    /*public List<BaseController> lastControllers = new List<BaseController>();
    public void SaveCars()
    {
        List<BaseController> baseControllers = new List<BaseController>(FindObjectsOfType<BaseController>().ToList());
        lastControllers = baseControllers;
    }
    private List<BaseController> GetBaseControllers()
    {
        return lastControllers;
    }*/
    public OneRoundRaceResultData GetCurOneRoundData()
    {
        return MyGameManager.instance.OneRoundRaceResultDatas[MyGameManager.instance.OneRoundRaceResultDatas.Count - 1];
    }
    private OneCarRaceResultData GetCarRaceResultData(string name)
    {
        OneRoundRaceResultData cur = GetCurOneRoundData();
        foreach (var i in cur.OneCarRaceResultDatas)
        {
            if (i.CarName == name) return i;
        }
        return null;
    }
    private OneCarRaceResultData GetCarRaceResultData(int place)
    {
        OneRoundRaceResultData cur = GetCurOneRoundData();
        foreach (var i in cur.OneCarRaceResultDatas)
        {
            if (i.Place == place) return i;
        }
        return null;
    }
    public string GetCarName(int place)
    {
        OneCarRaceResultData data = GetCarRaceResultData(place);
        if (data == null) return "";
        return data.CarName;
    }
    public float GetFinalTime(int place)
    {
        OneCarRaceResultData data = GetCarRaceResultData(place);
        if (data == null) return 0.0f;
        return data.FinalTime;
    }

    public float GetGapTime(int firstPlace, int nextPlace)
    {
        OneCarRaceResultData first = GetCarRaceResultData(firstPlace);
        OneCarRaceResultData next = GetCarRaceResultData(nextPlace);
        if(first == null || next == null) return 0.0f;
        return first.FinalTime - next.FinalTime;
    }

    public float GetGapTime(string firstCarName, string nextCarName)
    {
        OneCarRaceResultData first = GetCarRaceResultData(firstCarName);
        OneCarRaceResultData next = GetCarRaceResultData(nextCarName);
        if (first == null || next == null) return 0.0f;
        return first.FinalTime - next.FinalTime;
    }

    public float GetLapTime(string carName)
    {
        OneCarRaceResultData car = GetCarRaceResultData(carName);
        return car.FinalTime;
    }

    public int GetPlace(string carName)
    {
        OneCarRaceResultData car = GetCarRaceResultData(carName);
        return car.Place;
    }

    public bool IsFinishAfter(string firstCarName, string nextCarName)
    {
        return GetGapTime(firstCarName, nextCarName) > 0.0f;
    }
}
