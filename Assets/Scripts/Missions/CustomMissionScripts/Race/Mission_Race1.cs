using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission - Race1", menuName = "Missions/Race1", order = 9)]
public class Mission_Race1 : Mission
{
    public override string GetDescriptionText()
    {
        string ordinalString = GetOrdinalString(int1);

        return "Make <b>" + GetNameColour(name1) + name1 + defaultColor + "</b> finish <b>" + int1 + ordinalString +"</b>";
    }
    public override bool IsGoalReached()
    {
        return CarInfoSearch.instance.GetPlace(name1) == int1 ? true : false;
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
