using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race3", menuName = "Missions/Race3", order = 11)]
public class Mission_Race3 : Mission
{
    public override string GetDescriptionText()
    {
        return "Make the race last <b>more</b> than <b>" + int1 + " seconds</b>";
    }
    public override bool IsGoalReached()
    {
        return goalDouble > int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 < 30)
            return Mission.Difficulty.EASY;
        else if (int1 < 60)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.HARD;
    }
}
