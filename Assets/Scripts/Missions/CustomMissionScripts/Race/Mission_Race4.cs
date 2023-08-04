using Mono.Cecil.Cil;
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
            FixDuplicateVariableValues();

        description=GetDescriptionText();
    }

    private void FixDuplicateVariableValues()
    {
        // make list of all available race positions
        List<int> availableNumbers = new List<int>();
        for(int i=int3_min;i<int3_max+1;i++)
            availableNumbers.Add(i);

        // remove first variable race positon from available
        availableNumbers.Remove(int2);

        // pick random number from remaining available race positions
        int3 = availableNumbers[Random.Range(0, availableNumbers.Count)];
        Debug.Log("Race Mission 4 randomisation occured");
    }

    public override string GetDescriptionText(bool raceEnd = false)
    {
        string ordinalString1 = GetOrdinalString(int2);
        string ordinalString2 = GetOrdinalString(int3);

        // put position numbers in order
        string pos1;
        string pos2;
        if (int2 > int3)
        {
            pos1 = int3 + ordinalString2;
            pos2 = int2 + ordinalString1;
        }
        else
        {
            pos1 = int2 + ordinalString1;
            pos2 = int3 + ordinalString2;
        }

        return "Have at least <b>" + int1 + " seconds</b> gap between <b>" + pos1 +"</b> and <b>" + pos2 + "</b> place";
    }
    public override bool IsGoalReached()
    {
        return CarInfoSearch.instance.GetGapTime(varExtra5, int3) >= int1 ? true : false;
    }
    public override Mission.Difficulty GetDifficulty()
    {
        if ((int2 == 1 && int3 == 4) || (int2 == 4 && int3 == 1))
            return Mission.Difficulty.EASY;
        else
            return Mission.Difficulty.HARD;
    }

    private string GetOrdinalString(int number)
    {
        int lastDigit = number % 10;
        int secondLastDigit = (number / 10) % 10;

        if (secondLastDigit == 1)
        {
            return "th";
        }

        switch (lastDigit)
        {
            case 1:
                return "st";
            case 2:
                return "nd";
            case 3:
                return "rd";
            default:
                return "th";
        }
    }
}
