using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race5", menuName = "Missions/Race5", order = 12)]
public class Mission_Race5 : Mission
{
    public override string GetDescriptionText()
    {
        return "Have at least <b>" + int1 + " seconds</b> gap between <b>" + name1 + "</b> and <b>" + name2 + "</b>";
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
