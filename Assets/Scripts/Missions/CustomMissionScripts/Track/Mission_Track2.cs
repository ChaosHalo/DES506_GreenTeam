using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track2", menuName = "Missions/Track2", order = 2)]
public class Mission_Track2 : Mission
{
    public override string GetDescriptionText(bool raceEnd = false)
    {
        return "Place a <b>" + trackType.ToString() + "</b> track piece";
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
