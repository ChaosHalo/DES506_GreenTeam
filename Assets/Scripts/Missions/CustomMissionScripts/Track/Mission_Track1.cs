using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track1", menuName = "Missions/Track1", order = 1)]
public class Mission_Track1 : Mission
{
    public override string GetDescriptionText(bool raceEnd = false) 
    {
        return "Place <b>" + int1 + " "+ trackType.ToString() + "</b> track pieces";
    }
    public override bool IsGoalReached() 
    {
        return goalInt >= int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 <= 5)
            return Mission.Difficulty.EASY;
        else if (int1 <= 10)
            return Mission.Difficulty.MEDIUM;
        else 
            return Mission.Difficulty.HARD;
    }
    public override string GetProgressString()
    {
        int currentProgress = Mathf.Clamp(goalInt, 0, int1);
        string finalColour = IsGoalReached() ? colourComplete : colourInProgress;
        return finalColour + "<b> (" + currentProgress + "/" + int1 + ")";
    }
}
