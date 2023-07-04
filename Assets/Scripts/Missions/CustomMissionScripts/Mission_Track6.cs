using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track6", menuName = "Missions/Track6", order = 6)]
public class Mission_Track6 : Mission
{
    public override string GetDescriptionText()
    {
        return "Use the " + terrainType.ToString() + " terrain type";
    }
    public override bool IsGoalReached()
    {
        return goalInt > var1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        return Mission.Difficulty.EASY;
    }
}
