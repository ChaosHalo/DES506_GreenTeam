using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track8", menuName = "Missions/Track8", order = 8)]
public class Mission_Track8 : Mission
{
    public override void InitialiseMission()
    {
        base.InitialiseMission();

        int minRoll = var1_min;
        int maxRoll = (int)(MyGameManager.instance.GetCurrencyManager().GetPlayerCurrency() * 0.75f);
        int finalResult = Random.Range(minRoll, maxRoll) / 100;
        var1 = finalResult * 100;

        // set variables
        missionDifficulty = GetDifficulty();
        description = GetDescriptionText();
        rewardCurrency = CalculateReward();
    }
    public override string GetDescriptionText()
    {
        return "Spend <b>more</b> than " + var1 + " currency";
    }
    public override bool IsGoalReached()
    {
        Debug.Log(goalInt);
        return goalInt > var1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (var1 <= 1500)
            return Mission.Difficulty.EASY;
        else if (var1 <= 2500)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.HARD;
    }
}
