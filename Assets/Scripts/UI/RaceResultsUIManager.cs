using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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

    public List<GameObject> LeaderboardInfos = new List<GameObject>();
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
        // string s = "";
        for (int i = 0; i < 4; i++)
        {
            int place = i + 1;
            string name = CarInfoSearch.instance.GetCarName(place);
            float finalTime = CarInfoSearch.instance.GetFinalTime(place);
            
            OneCarRaceResultData raceResultData = new OneCarRaceResultData(name, place, finalTime);
            tempCarDatas.Add(raceResultData);

            // 排名背景图
            LeaderboardInfos[i].GetComponent<Image>().sprite = MyGameManager.instance.GetCarInfoScriptableObjectByName(name).CarUIInfo.RankImage;
            // 信息
            LeaderboardInfos[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = name + " - " + finalTime + " sec";
            // 排名
            LeaderboardInfos[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = place.ToString();
            //s += string.Format("{0} | {1} {2} - {3} sec\n", place, name, place, finalTime);
        }
        //PlaceholderScoreboard.GetComponent<TextMeshProUGUI>().text = s;
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
            MyGameManager.instance.OnEndSeason(EndOfSeason.gameObject);
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
