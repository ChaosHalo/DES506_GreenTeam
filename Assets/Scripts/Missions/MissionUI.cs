using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MissionUI : MonoBehaviour
{
    public TMP_Text[] missionTexts = new TMP_Text[3];
    public TMP_Text[] rewardTexts = new TMP_Text[3];
    public MissionDifficultyIndicator[] missionDifficultyIndicators = new MissionDifficultyIndicator[3];

    [SerializeField] private GameObject completedMissionUI;
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private CurrencyManager currencyManager;

    private bool isMissionCompelteUIOpen = false;
    private string missionProgressColor = "<color=#798F9D>";


    private void Update()
    {
        if (isMissionCompelteUIOpen == true)
            if (Input.GetMouseButton(0))
                completedMissionUI.SetActive(false);

        UpdateMissionTexts();
        UpdateMissionDifficultyIndicators();
        UpdateRewardTexts();
    }

    private void UpdateMissionTexts()
    {
        for (int i = 0; i < 3; i++)
        {
            if (missionTexts[i] != null && MyGameManager.instance.missionManager.HasMission(i))
            {
                Mission currentMission = MyGameManager.instance.missionManager.GetMission(i);
                string missionProgress = currentMission.GetProgressString();
                missionTexts[i].text = currentMission.description + "<b>" + missionProgressColor + missionProgress;
            }
        }
    }

    private void UpdateMissionDifficultyIndicators()
    {
        for (int i = 0; i < 3; i++)
        {
            if (missionDifficultyIndicators[i] != null && MyGameManager.instance.missionManager.HasMission(i))
            {
                Mission currentMission = MyGameManager.instance.missionManager.GetMission(i);
                Mission.Difficulty missionDifficulty = currentMission.GetDifficulty();
                missionDifficultyIndicators[i].UpdateDifficulty(missionDifficulty);
            }
        }
    }

    private void UpdateRewardTexts()
    {
        for (int i = 0; i < 3; i++)
        {
            if (rewardTexts[i] != null && MyGameManager.instance.missionManager.HasMission(i))
            {
                Mission currentMission = MyGameManager.instance.missionManager.GetMission(i);
                int missionReward = currentMission.CalculateReward();
                rewardTexts[i].text = missionReward.ToString();
            }
        }
    }

    public void UpdateMissionStatusUI(RaceResultsUIManager raceResultsUI)
    {
        List<string> completedMissionDescriptions = new List<string>();
        List<string> missionRewards = new List<string>();
        List<bool> completedStatus = new List<bool>();
        int totalReward = 0;
        int missionCompletedNum = 0;
        for (int i = 0; i < 3; i++)
        {
            if (missionManager.HasMission(i))
            {
                Mission currentMission = missionManager.GetMission(i);

                string missionDescription = currentMission.GetDescriptionText(true);
                completedMissionDescriptions.Add(missionDescription);

                if (currentMission.IsGoalReached())
                {
                    HandleCompletedMission(currentMission, i);
                    totalReward += currentMission.rewardCurrency;
                    completedStatus.Add(true);
                    missionRewards.Add(currentMission.rewardCurrency.ToString());
                    missionCompletedNum++;
                }
                else
                {
                    completedStatus.Add(false);
                    missionRewards.Add("0");
                }
            }
        }

        UpdateMissionInfo(missionCompletedNum, totalReward);
        UpdateMissionStatusUIElements(raceResultsUI, completedMissionDescriptions, missionRewards, completedStatus, totalReward);
    }

    private void UpdateMissionInfo(int missionCompletedNum, int totalReward)
    {
        MyGameManager.MissionInfo missionInfo;
        missionInfo.TotalCompletedMissionNum = missionCompletedNum;
        missionInfo.TotalWinCurrency = totalReward;
        MyGameManager.instance.MissionInfos.Add(missionInfo);
    }
    private void HandleCompletedMission(Mission mission, int index)
    {
        Debug.Log("Completed Mission: " + mission.description);

        currencyManager.AddMissionCurrency(mission.rewardCurrency);

        missionManager.RerollMission(index);
    }

    private void UpdateMissionStatusUIElements(RaceResultsUIManager raceResultsUI, List<string> missionDescriptions, List<string> missionRewards, List<bool> completedStatus, int totalReward)
    {
        raceResultsUI.baseIncomeText.text = "BASE INCOME: " + currencyManager.GetWinCurrency().ToString();
        raceResultsUI.finalRewardText.text = "MONEY EARNED: " + (currencyManager.GetWinCurrency() + totalReward).ToString();

        for (int i = 0; i < 3; i++)
        {
            raceResultsUI.missionDescriptionsTexts[i].text = missionDescriptions[i] + " ( +" + missionRewards[i] + " )";
            raceResultsUI.completionCheckmarks[i].SetActive(completedStatus[i]);
        }
    }
}
