using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race2", menuName = "Missions/Race2", order = 10)]
public class Mission_Race2 : Mission
{
    public override string GetDescriptionText(bool raceEnd = false)
    {
        return "Make the race last <b>less</b> than <b>" + int1 + " seconds</b>";
    }
    public override bool IsGoalReached()
    {
        Debug.Log("Target: " + int1 + " Final: " + goalDouble);
        return goalDouble < int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 < 30)
            return Mission.Difficulty.HARD;
        else if (int1 < 60)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.EASY;
    }
}
