using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDifficultyIndicator : MonoBehaviour
{
    public List<Image> indicators = new List<Image>();

    public void UpdateDifficulty(Mission.Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Mission.Difficulty.EASY:
                indicators[0].enabled = true;
                indicators[1].enabled = false;
                indicators[2].enabled = false;
                break;
            case Mission.Difficulty.MEDIUM:
                indicators[0].enabled = false;
                indicators[1].enabled = true;
                indicators[2].enabled = false;
                break;
            case Mission.Difficulty.HARD:
                indicators[0].enabled = false;
                indicators[1].enabled = false;
                indicators[2].enabled = true;
                break;
        }
    }
}
