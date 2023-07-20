using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track6", menuName = "Missions/Track6", order = 6)]
public class Mission_Track6 : Mission
{
    public override string GetDescriptionText()
    {
        return "Place the <b>" + terrainType.ToString() + "</b> terrain type";
    }
    public override bool IsGoalReached()
    {
        return goalInt >= int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        return Mission.Difficulty.EASY;
    }
    public override string GetProgressString()
    {
        int currentProgress = Mathf.Clamp(goalInt, 0, int1);
        string finalColour = IsGoalReached() ? colourComplete : colourInProgress;
        return finalColour + "<b> (" + currentProgress + "/" + int1 + ")";
    }
}
