using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track4", menuName = "Missions/Track4", order = 4)]
public class Mission_Track4 : Mission
{
    public override void InitialiseMission()
    {
        base.InitialiseMission();

        int currentMoney = MyGameManager.instance.GetCurrencyManager().GetPlayerCurrency();
        int minTrackCost = 100;

        int minGoal = 5;
        int maxGoal = (currentMoney / minTrackCost) - 5;
        int1 = Random.Range(minGoal, maxGoal);

        // set variables
        missionDifficulty = GetDifficulty();
        description = GetDescriptionText();
        rewardCurrency = CalculateReward();
    }
    public override string GetDescriptionText(bool raceEnd = false)
    {
        return "Place <b>more</b> than <b>" + int1 + " track pieces</b> total";
    }
    public override bool IsGoalReached()
    {
        return goalInt > int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        int currentMoney = MyGameManager.instance.GetCurrencyManager().GetPlayerCurrency();
        int minTrackCost = 100;

        float easyThreshold = currentMoney / (minTrackCost * 3);
        float mediumThreshhold = currentMoney / (minTrackCost * 2);

        if (int1 < easyThreshold)
            return Mission.Difficulty.EASY;
        else if (int1 <= mediumThreshhold)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.HARD;
    }
    public override string GetProgressString()
    {
        string finalColour = IsGoalReached() ? colourComplete : colourInProgress;
        return finalColour + "<b> (" + goalInt + "/" + (int1+1) + ")";
    }
}
