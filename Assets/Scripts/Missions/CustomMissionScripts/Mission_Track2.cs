using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Track2", menuName = "Missions/Track2", order = 2)]
public class Mission_Track2 : Mission
{
    public override string GetDescriptionText()
    {
        return "Use a " + trackType.ToString() + " track piece";
    }
    public override bool IsReached() 
    {
        return goalInt >= var1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        return Mission.Difficulty.EASY;
    }
}
