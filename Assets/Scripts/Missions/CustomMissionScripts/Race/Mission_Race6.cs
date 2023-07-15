using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race6", menuName = "Missions/Race6", order = 14)]
public class Mission_Race6 : Mission
{
    public override string GetDescriptionText()
    {
        return "Make <b>" + name1 + "</b> finish after <b>" + name2 + "</b>";
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
