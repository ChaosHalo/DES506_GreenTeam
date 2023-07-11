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
    protected override void ChangeState()
    {
        // 非赛季
        if(MyGameManager.instance.GameRound % MyGameManager.instance.Season != 0)
        {
            base.ChangeState();
        }
        // 赛季特殊处理
        EndOfSeason.gameObject.SetActive(true);
    }
    protected override void WinCurrency()
    {
        // 非赛季
        if (MyGameManager.instance.GameRound % MyGameManager.instance.Season != 0)
        {
            base.WinCurrency();
        }
        // 赛季特殊处理
    }
}
