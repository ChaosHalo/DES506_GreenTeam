using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MissionGrouping
{
    [SerializeField]
    public Mission mission;
    [SerializeField]
    public int grouping;
}

public class MissionManager : MonoBehaviour
{
    [Header("Mission Pool")]
    public List<MissionGrouping> missionPool = new List<MissionGrouping>();

    [Header("Current Missions")]
    public Mission[] currentMissions = new Mission[3];

    [Header("UI")]
    public MissionUI missionUI;

    private CurrencyManager currencyManager;

    [SerializeField]
    private List<MissionGrouping> availableMissions = new List<MissionGrouping>();

    private void Update()
    {
        if (missionUI == null)
            missionUI = FindObjectOfType<MissionUI>();
        if (currencyManager == null)
            currencyManager = FindObjectOfType<CurrencyManager>();

        if (Input.GetKeyDown(KeyCode.T))
            CheckForCompletedMissions();
    }

    public void InitialiseMissions()
    {
        if(availableMissions.Count == 0)
            availableMissions.AddRange(missionPool);

        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] == null)
            {
                CreateNewMission(i);
            }
        }
    }

    public void CheckForCompletedMissions()
    {
        List<string> descriptions = new List<string>();
        List<int> rewards = new List<int>();
        List<Color> colours = new List<Color>();

        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].IsGoalReached())
                {
                    Debug.Log("Completed Mission: "+ currentMissions[i].description);

                    if (currencyManager != null)
                        currencyManager.AddMissionCurrency(currentMissions[i].rewardCurrency);

                    descriptions.Add(currentMissions[i].description);
                    rewards.Add(currentMissions[i].rewardCurrency);
                    colours.Add(currentMissions[i].GetDifficultyColour());

                    RerollMission(i);
                }
            }
        }

        if(missionUI != null) 
            missionUI.ShowCompletedMissions(descriptions, rewards, colours);
    }

    public void CreateNewMission(int index)
    {
        currentMissions[index] = Instantiate(availableMissions[Random.Range(0, availableMissions.Count)].mission);
        currentMissions[index].InitialiseMission();
        currentMissions[index].index = index;
        OnAddGrouping(currentMissions[index].grouping);
    }

    public void RerollMission(int index)
    {
        int rerolledGrouping = currentMissions[index].grouping;
        currentMissions[index] = null;
        CreateNewMission(index);
        OnRemoveGrouping(rerolledGrouping);
    }

    private void OnAddGrouping(int grouping)
    {
        availableMissions.RemoveAll(item => item.grouping==grouping);
    }

    private void OnRemoveGrouping(int grouping)
    {
        foreach (MissionGrouping item in missionPool)
            if (item.grouping == grouping)
                availableMissions.Add(item);
    }




    #region MISSION_EVENTS
    #region MISSION_EVENTS_TRACK
    public void Event_PlaceTrack()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track3" || currentMissions[i].title == "Track4")
                {
                    currentMissions[i].goalInt++;
                }
            }
        }
    }
    public void Event_PlaceTrackStraight()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track1" || currentMissions[i].title == "Track2")
                {
                    if(currentMissions[i].trackType == ObjectData.TrackType.Straight)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    public void Event_PlaceTrackCurve()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track1" || currentMissions[i].title == "Track2")
                {
                    if (currentMissions[i].trackType == ObjectData.TrackType.Curve)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    public void Event_PlaceTrackLoop()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track1" || currentMissions[i].title == "Track2")
                {
                    if (currentMissions[i].trackType == ObjectData.TrackType.Loop)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    #endregion

    #region MISSION_EVENTS_TERRAIN
    public void Event_PlaceTerrainGrass()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track5")
                {
                    currentMissions[i].varExtra1++;
                }
                if (currentMissions[i].title == "Track6")
                {
                    if (currentMissions[i].terrainType == ObjectData.TerrainType.Grass)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    public void Event_PlaceTerrainDesert()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track5")
                {
                    currentMissions[i].varExtra2++;
                }
                if (currentMissions[i].title == "Track6")
                {
                    if (currentMissions[i].terrainType == ObjectData.TerrainType.Desert)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    public void Event_PlaceTerrainSnow()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track5")
                {
                    currentMissions[i].varExtra3++;
                }
                if (currentMissions[i].title == "Track6")
                {
                    if (currentMissions[i].terrainType == ObjectData.TerrainType.Snow)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    public void Event_PlaceTerrainSea()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track5")
                {
                    currentMissions[i].varExtra4++;
                }
                if (currentMissions[i].title == "Track6")
                {
                    if (currentMissions[i].terrainType == ObjectData.TerrainType.Sea)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    public void Event_PlaceTerrainMountain()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track5")
                {
                    currentMissions[i].varExtra5++;
                }
                if (currentMissions[i].title == "Track6")
                {
                    if (currentMissions[i].terrainType == ObjectData.TerrainType.Mountain)
                    {
                        currentMissions[i].goalInt++;
                    }
                }
            }
        }
    }
    #endregion

    #region MISSION_EVENT_CURRENCY
    public void Event_SpendCurrency(Component sender, object data)
    {
        List<string> eligibleMissions = new() { "Track7", "Track8" };

        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                foreach (string missionName in eligibleMissions)
                {
                    if (currentMissions[i].title == missionName)
                    {
                        if (data is int)
                        {
                            currentMissions[i].goalInt += (int)data;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region RACE
    public void Event_RaceEndTime(Component sender, object data)
    {
        List<string> eligibleMissions = new() { "Race2", "Race3" };

        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                foreach (string missionName in eligibleMissions)
                {
                    if (currentMissions[i].title == missionName)
                    {
                        if (data is double)
                        {
                            currentMissions[i].goalDouble = (double)data;
                        }
                    }
                }
            }
        }
    }
    public void Event_RaceEndCarInfo(Component sender, object data)
    {
        List<string> eligibleMissions = new() { "Race1", "Race4", "Race5", "Race6" };

        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                foreach(string missionName in eligibleMissions)
                {
                    if (currentMissions[i].title == missionName)
                    {
                        if (sender != null)
                        {
                            CarInfoSerach carInfoSerach = sender as CarInfoSerach;
                            if (carInfoSerach != null)
                            {
                                currentMissions[i].carInfoSerach = carInfoSerach;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
    #endregion
}
