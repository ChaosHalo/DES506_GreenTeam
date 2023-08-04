using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race5", menuName = "Missions/Race5", order = 12)]
public class Mission_Race5 : Mission
{
    public override string GetDescriptionText(bool raceEnd = false)
    {
        string defaultTextColor = raceEnd ? defaultColor_Light : defaultColor_Dark;

        return "Have at least <b>" + int1 + " seconds</b> gap between <b>" + GetNameColour(name1) + name1 + defaultTextColor + "</b> and <b>" + GetNameColour(name2) + name2 + "</b>";
    }
    public override bool IsGoalReached()
    {
        return CarInfoSearch.instance.GetGapTime(name1, name2) >= int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        return Mission.Difficulty.MEDIUM;
    }
}
