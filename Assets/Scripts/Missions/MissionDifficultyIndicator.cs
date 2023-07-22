using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDifficultyIndicator : MonoBehaviour
{
    public List<Image> indicators = new List<Image>();

    public void UpdateDifficulty(Mission.Difficulty difficulty)
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].enabled = (i == (int)difficulty);
        }
    }
}
