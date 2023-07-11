using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MissionUI : MonoBehaviour
{
    public TMP_Text[] missionTexts = new TMP_Text[3];
    public TMP_Text[] rewardTexts = new TMP_Text[3];
    public MissionDifficultyIndicator[] missionDifficultyIndicators = new MissionDifficultyIndicator[3];

    [SerializeField]
    private GameObject completedMissionUI;

    private float hideDuration = 5f;

    [SerializeField]
    private List<GameObject> completedMissionTabs = new List<GameObject>();
    [SerializeField]
    private List<Image> completedMissionDifficultyIndicators= new List<Image>();

    [SerializeField]
    private Text rewardText;

    bool isMissionCompelteUIOpen = false;


    public void CustomUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            if (missionTexts[i].text != null)
                missionTexts[i].text = MyGameManager.instance.missionManager.currentMissions[i].GetDescriptionText();
            if (missionDifficultyIndicators[i] != null)
                missionDifficultyIndicators[i].UpdateDifficulty(MyGameManager.instance.missionManager.currentMissions[i].GetDifficulty());
            if (rewardTexts[i].text != null)
                rewardTexts[i].text = MyGameManager.instance.missionManager.currentMissions[i].CalculateReward().ToString();
        }
    }

    private void Update()
    {
        if(isMissionCompelteUIOpen==true)
            if (Input.GetMouseButton(0))
                completedMissionUI.SetActive(false);
    }

    public void ShowCompletedMissions(List<string> descriptions, List<int> rewards, List<Color> colours)
    {
        return;

        if (descriptions.Count <= 0)
            return;

        int totalReward = 0;

        completedMissionUI.SetActive(true);
        foreach(GameObject tab in completedMissionTabs)
            tab.SetActive(false);

        for(int i=0;i<descriptions.Count; i++)
        {
            completedMissionTabs[i].SetActive(true);
            completedMissionTabs[i].GetComponentInChildren<TMP_Text>().text = descriptions[i];
            completedMissionDifficultyIndicators[i].color = colours[i];
            totalReward += rewards[i];
        }

        rewardText.text = "+" + totalReward;

        StartCoroutine(HideUIAfterDuration());
    }

    private IEnumerator HideUIAfterDuration()
    {
        isMissionCompelteUIOpen = true;
        yield return new WaitForSeconds(hideDuration);
        completedMissionUI.SetActive(false);
        isMissionCompelteUIOpen = false;
    }
}
