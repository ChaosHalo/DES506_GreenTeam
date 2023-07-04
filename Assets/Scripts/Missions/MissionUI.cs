using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MissionUI : MonoBehaviour
{
    public Text[] missionTexts = new Text[3];
    public Image[] missionDifficultyIndicators = new Image[3];
    MissionManager missionManager;

    private void Update()
    {
        if(missionManager==null)
            missionManager=GameObject.FindObjectOfType<MissionManager>();
    }

    private void FixedUpdate()
    {
        if (missionManager != null)
        {
            for(int i = 0; i < 3; i++)
            {
                missionTexts[i].text = missionManager.currentMissions[i].GetDescriptionText();
                missionDifficultyIndicators[i].color = missionManager.currentMissions[i].GetDifficultyColour();
            }
        }
    }
}
