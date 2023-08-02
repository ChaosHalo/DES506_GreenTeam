using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track5", menuName = "Missions/Track5", order = 5)]
public class Mission_Track5 : Mission
{
    public override string GetDescriptionText()
    {
        string pluralString = "types";
        if (int1 == 1)
            pluralString = "type";

        return "Place <b>" + int1 + " unique terrain</b> " + pluralString;
    }
    public override bool IsGoalReached()
    {
        return GetUniquePieceesCount() >= int1 ? true: false;
    }

    public int GetUniquePieceesCount()
    {
        int typesUsed = 0;

        if (varExtra1 > 0)
            typesUsed++;
        if (varExtra2 > 0)
            typesUsed++;
        if (varExtra3 > 0)
            typesUsed++;
        if (varExtra4 > 0)
            typesUsed++;
        if (varExtra5 > 0)
            typesUsed++;

        return typesUsed;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 <= 2)
            return Mission.Difficulty.EASY;
        else if (int1 <= 4)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.HARD;
    }
    public override string GetProgressString()
    {
        int currentProgress = Mathf.Clamp(GetUniquePieceesCount(), 0, int1);
        string finalColour = IsGoalReached() ? colourComplete : colourInProgress;
        return finalColour + "<b> (" + currentProgress + "/" + int1 + ")";
    }
}
