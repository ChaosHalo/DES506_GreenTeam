using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RaceResultsUIManager : AllUIManager
{
    public GameObject PlaceholderScoreboard;
    public EndOfSeasonUIManager EndOfSeason;

    public TMP_Text baseIncomeText;
    public TMP_Text missionRewardText;
    public TMP_Text finalRewardText;
    public TMP_Text totalText;
    public List<TMP_Text> missionDescriptionsTexts = new List<TMP_Text>();
    public List<GameObject> completionCheckmarks = new List<GameObject>();


    private void Start()
    {
        //UpdatePlaceholderScoreboard();
    }
    private void OnEnable()
    {
        //UpdatePlaceholderScoreboard();
    }

    public void UpdatePlaceholderScoreboard()
    {
        MyGameManager.instance.missionManager.CheckForCompletedMissions(this);

        List<OneCarRaceResultData> tempCarDatas = new(); 
        string s = "";
        for (int i = 0; i < 4; i++)
        {
            int place = i + 1;
            string name = CarInfoSearch.instance.GetCarName(place);
            float finalTime = CarInfoSearch.instance.GetFinalTime(place);
            OneCarRaceResultData raceResultData = new OneCarRaceResultData(name, place, finalTime);
            tempCarDatas.Add(raceResultData);
            s += string.Format("{0} | {1} {2} - {3} sec\n", place, name, place, finalTime);
        }
        PlaceholderScoreboard.GetComponent<TextMeshProUGUI>().text = s;
    }
    protected override void ChangeState()
    {
        // 非赛季
        if(MyGameManager.instance.GameRound % MyGameManager.instance.Season != 0)
        {
            base.ChangeState();
        }
        else
        {
            // 赛季特殊处理
            EndOfSeason.gameObject.SetActive(true);
        }
        
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
