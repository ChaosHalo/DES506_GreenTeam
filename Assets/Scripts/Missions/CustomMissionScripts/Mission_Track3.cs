using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track3", menuName = "Missions/Track3", order = 3)]
public class Mission_Track3 : Mission
{
    public override string GetDescriptionText()
    {
        return "Place <b>less</b> than <b>" + var1 + " track pieces</b> total";
    }
    public override bool IsGoalReached()
    {
        return goalInt < var1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (var1 <= 25)
            return Mission.Difficulty.HARD;
        else if (var1 <= 45)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.EASY;
    }
}
