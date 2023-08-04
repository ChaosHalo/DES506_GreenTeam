using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race6", menuName = "Missions/Race6", order = 14)]
public class Mission_Race6 : Mission
{
    public override string GetDescriptionText(bool raceEnd = false)
    {
        string defaultTextColor = raceEnd ? defaultColor_Light : defaultColor_Dark;

        return "Make <b>" + GetNameColour(name1) + name1 + defaultTextColor + "</b> finish after <b>" + GetNameColour(name2) + name2 + defaultTextColor + "</b>";
    }
    public override bool IsGoalReached()
    {
        return CarInfoSearch.instance.IsFinishAfter(name1, name2) ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        return Mission.Difficulty.MEDIUM;
    }
}
