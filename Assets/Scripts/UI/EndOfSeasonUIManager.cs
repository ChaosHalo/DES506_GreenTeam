using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndOfSeasonUIManager : AllUIManager
{
    public GameObject BestDriverName;
    public GameObject WorstDriverName;
    public GameObject BestDriverTime;
    public GameObject WorstDriverTime;

    private void Start()
    {
        UpdateBestDriverName();
        UpdateWorstDriverName();
        UpdateBestDriverTime();
        UpdateWorstDriverTime();
    }
    private List<OneRoundRaceResultData> GetSeasonRoundData()
    {
        List<OneRoundRaceResultData> oneRoundRaceResultDatas = MyGameManager.instance.OneRoundRaceResultDatas;
        return oneRoundRaceResultDatas.GetRange(oneRoundRaceResultDatas.Count - MyGameManager.instance.Season, MyGameManager.instance.Season);
    }
    private OneRoundRaceResultData GetMergeRoundData()
    {
        List<OneRoundRaceResultData> Season = GetSeasonRoundData();
        OneRoundRaceResultData oneRoundRaceResultData = new();
        foreach(var raceData in Season)
        {
            var tempCars = raceData.OneCarRaceResultDatas;
            foreach(var oneCarData in tempCars)
            {
                if (!oneRoundRaceResultData.ContainCar(oneCarData.CarName))
                {
                    oneRoundRaceResultData.AddCarData(oneCarData);
                }
                else
                {
                    oneRoundRaceResultData.MergeCarFinalTimeData(oneCarData);
                }
            }
        }
        oneRoundRaceResultData.UpdateCarsRank();
        return oneRoundRaceResultData;
    }
    private void UpdateBestDriverName()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        string name = oneRoundRaceResultData.OneCarRaceResultDatas[0].CarName;
        BestDriverName.GetComponent<TextMeshProUGUI>().text = "Best driver:" + name;
    }
    private void UpdateWorstDriverName()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        string name = oneRoundRaceResultData.OneCarRaceResultDatas[oneRoundRaceResultData.OneCarRaceResultDatas.Count - 1].CarName;
        WorstDriverName.GetComponent<TextMeshProUGUI>().text = "Worst driver:" + name;
    }
    private void UpdateBestDriverTime()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        float time = oneRoundRaceResultData.OneCarRaceResultDatas[0].FinalTime;
        BestDriverTime.GetComponent<TextMeshProUGUI>().text = "Best time:" + time.ToString("f3");
    }
    private void UpdateWorstDriverTime()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        float time = oneRoundRaceResultData.OneCarRaceResultDatas[oneRoundRaceResultData.OneCarRaceResultDatas.Count - 1].FinalTime;
        WorstDriverTime.GetComponent<TextMeshProUGUI>().text = "Worst time:" + time.ToString("f3");
    }
}
