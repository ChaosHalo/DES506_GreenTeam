using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track7", menuName = "Missions/Track7", order = 7)]
public class Mission_Track7 : Mission
{
    public override void InitialiseMission()
    {
        base.InitialiseMission();

        CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();
        if(currencyManager != null)
        {
            int minRoll = var1_min;
            int maxRoll = (int)(currencyManager.GetPlayerCurrency() * 0.75f);
            int finalResult = UnityEngine.Random.Range(minRoll, maxRoll) / 100;
            finalResult *= 100;
            var1 = finalResult;
        }

        // set variables
        missionDifficulty = GetDifficulty();
        description = GetDescriptionText();
        rewardCurrency = CalculateReward();
    }
    public override string GetDescriptionText()
    {
        return "Spend less than " + var1 + " currency";
    }
    public override bool IsGoalReached()
    {
        return goalInt < var1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (var1 <= 1500)
            return Mission.Difficulty.HARD;
        else if (var1 <= 2500)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.EASY;
    }
}
