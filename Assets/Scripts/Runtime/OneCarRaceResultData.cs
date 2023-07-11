using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneCarRaceResultData
{
    public string CarName;
    public int Rank;
    public float FinalTime;
    public OneCarRaceResultData(string carName, int rank, float finalTime)
    {
        CarName = carName;
        Rank = rank;
        FinalTime = finalTime;
    }
}

[System.Serializable]
public class OneRoundRaceResultData
{
    public List<OneCarRaceResultData> OneCarRaceResultDatas;
    public bool ContainCar(string carName)
    {
        if(OneCarRaceResultDatas.Count == 0) return false;
        foreach (var i in OneCarRaceResultDatas)
        {
            if(i.CarName == carName) return true;
        }
        return false;
    }
    public void AddCarData(OneCarRaceResultData oneCarRaceResultData)
    {
        OneCarRaceResultDatas.Add(oneCarRaceResultData);
    }
    /// <summary>
    /// 合并车辆时间
    /// </summary>
    /// <param name="oneCarRaceResultData"></param>
    public void MergeCarFinalTimeData(OneCarRaceResultData oneCarRaceResultData)
    {
        for(int i = 0; i < OneCarRaceResultDatas.Count; i++)
        {
            OneCarRaceResultData curCar = OneCarRaceResultDatas[i];
            if (oneCarRaceResultData.CarName == curCar.CarName)
            {
                OneCarRaceResultData temp = new(
                    curCar.CarName,
                    curCar.Rank,
                    curCar.FinalTime + oneCarRaceResultData.FinalTime);
                OneCarRaceResultDatas[i] = temp;
                return;
            }
        }
    }
    public void UpdateCarsRank()
    {
        // Sort the list based on FinalTime in ascending order
        OneCarRaceResultDatas.Sort((x, y) => x.FinalTime.CompareTo(y.FinalTime));

        // Update the ranks based on the sorted order
        for (int i = 0; i < OneCarRaceResultDatas.Count; i++)
        {
            OneCarRaceResultData carData = OneCarRaceResultDatas[i];
            carData.Rank = i + 1; // Update the rank starting from 1
            OneCarRaceResultDatas[i] = carData; // Update the modified car data in the list
        }
    }

    public OneRoundRaceResultData()
    {
        OneCarRaceResultDatas = new();
    }
    public OneRoundRaceResultData(List<OneCarRaceResultData> oneCarRaceResultDatas)
    {
        OneCarRaceResultDatas = oneCarRaceResultDatas;
    }
}   
