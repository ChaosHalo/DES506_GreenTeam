using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track8", menuName = "Missions/Track8", order = 8)]
public class Mission_Track8 : Mission
{
    public override void InitialiseMission()
    {
        base.InitialiseMission();

        int minRoll = int1_min;
        int maxRoll = (int)(MyGameManager.instance.GetCurrencyManager().GetPlayerCurrency() * 0.75f);
        int finalResult = Random.Range(minRoll, maxRoll) / 100;
        int1 = finalResult * 100;

        // set variables
        missionDifficulty = GetDifficulty();
        description = GetDescriptionText();
        rewardCurrency = CalculateReward();
    }
    public override string GetDescriptionText()
    {
        return "Spend <b>more than " + int1 + "</b> currency this round";
    }
    public override bool IsGoalReached()
    {
        return goalInt > int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 <= 1500)
            return Mission.Difficulty.EASY;
        else if (int1 <= 2500)
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
