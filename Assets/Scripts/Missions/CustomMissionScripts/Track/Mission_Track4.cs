using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track4", menuName = "Missions/Track4", order = 4)]
public class Mission_Track4 : Mission
{
    public override string GetDescriptionText()
    {
        return "Place <b>more</b> than <b>" + int1 + " track pieces</b> total";
    }
    public override bool IsGoalReached()
    {
        return goalInt > int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 <= 40)
            return Mission.Difficulty.EASY;
        else if (int1 <= 55)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.HARD;
    }
}
