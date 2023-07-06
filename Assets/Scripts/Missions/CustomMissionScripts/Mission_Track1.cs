using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track1", menuName = "Missions/Track1", order = 1)]
public class Mission_Track1 : Mission
{
    public override string GetDescriptionText() 
    {
        return "Use " + var1 + " <b>"+ trackType.ToString() + "</b> track pieces";
    }
    public override bool IsGoalReached() 
    {
        return goalInt >= var1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (var1 <= 5)
            return Mission.Difficulty.EASY;
        else if (var1 <= 10)
            return Mission.Difficulty.MEDIUM;
        else 
            return Mission.Difficulty.HARD;
    }
}
