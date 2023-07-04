using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track5", menuName = "Missions/Track5", order = 5)]
public class Mission_Track5 : Mission
{
    public override string GetDescriptionText()
    {
        return "Use " + var1 + " terrain types";
    }
    public override bool IsGoalReached()
    {
        int typesUsed = 0;

        if(varExtra1>0)
            typesUsed++;
        if (varExtra2 > 0)
            typesUsed++;
        if (varExtra3 > 0)
            typesUsed++;
        if (varExtra4 > 0)
            typesUsed++;
        if (varExtra5 > 0)
            typesUsed++;

        return typesUsed > var1 ? true: false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (var1 <= 1)
            return Mission.Difficulty.EASY;
        else if (var1 <= 3)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.HARD;
    }
}
