using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RaceResultsUIManager : AllUIManager
{
    public GameObject PlaceholderScoreboard;
    public EndOfSeasonUIManager EndOfSeason;
    private void Start()
    {
        UpdatePlaceholderScoreboard();
    }

    public void UpdatePlaceholderScoreboard()
    {
        List<OneCarRaceResultData> tempCarDatas = new(); 
        string s = "";
        for (int i = 0; i < 4; i++)
        {
            int place = i + 1;
            string name = CarInfoSerach.instance.GetCarName(place);
            float finalTime = CarInfoSerach.instance.GetFinalTime(place);
            OneCarRaceResultData raceResultData = new OneCarRaceResultData(name, place, finalTime);
            tempCarDatas.Add(raceResultData);
            s += string.Format("{0} | {1} {2} - {3} sec\n", place, name, place, finalTime);
        }
        PlaceholderScoreboard.GetComponent<TextMeshProUGUI>().text = s;
        MyGameManager.instance.AddRaceResult(new OneRoundRaceResultData(tempCarDatas));
    }
}
