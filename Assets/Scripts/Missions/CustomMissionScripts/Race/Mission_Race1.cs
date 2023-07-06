using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race1", menuName = "Missions/Race1", order = 9)]
public class Mission_Race1 : Mission
{
    public override string GetDescriptionText()
    {
        string numberEnd = "th";
        if (int1 == 1)
            numberEnd = "st";
        else if (int1 == 2)
            numberEnd = "nd";
        else if (int1 == 3)
            numberEnd = "rd";

        return "Make <b>" + name1 + "</b> finish <b>" + int1 + numberEnd +"</b>";
    }
    public override bool IsGoalReached()
    {
        if (carInfoSerach == null)
            return false;

        return carInfoSerach.GetRank(name1, 0) == int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int1 < 2)
            return Mission.Difficulty.EASY;
        else if (int1 < 4)
            return Mission.Difficulty.MEDIUM;
        else
            return Mission.Difficulty.EASY;
    }
}
