using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race4", menuName = "Missions/Race4", order = 12)]
public class Mission_Race4 : Mission
{
    public override void InitialiseMission()
    {
        base.InitialiseMission();
        if (int2 == int3)
            RandomiseVar4();
    }

    private void RandomiseVar4()
    {
        int3 = UnityEngine.Random.Range(int3_min, int3_max + 1);
        if (int2 == int3)
            RandomiseVar4();
    }

    public override string GetDescriptionText()
    {
        string numberEnd1 = "th";
        if (int2 == 1)
            numberEnd1 = "st";
        else if (int2 == 2)
            numberEnd1 = "nd";
        else if (int2 == 3)
            numberEnd1 = "rd";


        string numberEnd2 = "th";
        if (int3 == 1)
            numberEnd2 = "st";
        else if (int3 == 2)
            numberEnd2 = "nd";
        else if (int3 == 3)
            numberEnd2 = "rd";


        // put position numbers in order
        string pos1;
        string pos2;
        if (int2 > int3)
        {
            pos1 = int3 + numberEnd2;
            pos2 = int2 + numberEnd1;
        }
        else
        {
            pos1 = int2 + numberEnd1;
            pos2 = int3 + numberEnd2;
        }

        return "Have at least <b>" + int1 + " seconds</b> gap between <b>" + pos1 +"</b> and <b>" + pos2 + "</b> place";
    }
    public override bool IsGoalReached()
    {
        if (carInfoSerach == null)
            return false;

        return carInfoSerach.GetGapTime(varExtra5, int3, 0) >= int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if (int2 == 1 && int3 == 4)
            return Mission.Difficulty.EASY;
        else if (int2 == 4 && int3 == 1)
                return Mission.Difficulty.EASY;
        else
            return Mission.Difficulty.HARD;
    }
}
