using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Missions;

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
    [Header("Events")]
    public MissionEvent onRaceEndTime;
    public MissionEvent onRaceEndCarInfo;

    [Header("Mission Pool")]
    public List<MissionGrouping> missionPool = new List<MissionGrouping>();

    [Header("Current Missions")]
    public Mission[] currentMissions = new Mission[3];

    [Header("UI")]
    public MissionUI missionUI;

    [SerializeField] private List<MissionGrouping> availableMissions = new List<MissionGrouping>();

    [SerializeField] public TMP_Text rerollCostText;

    public int rerollCost = 100;
    public int rerollCount = 0;

    private void Start()
    {
        //update cost text in UI
        UpdateRerollText();
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
    internal bool HasMission(int index)
    {
        return currentMissions.Length > index && currentMissions[index] != null;
    }

    internal Mission GetMission(int index)
    {
        return currentMissions[index];
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

    public void RerollAllMissions()
    {
        CurrencyManager currencyManager = MyGameManager.instance.GetCurrencyManager();
        int newCost = (rerollCount) * rerollCost;

        if (currencyManager.CanAfford(newCost))
        {
            // spend money
            currencyManager.MakePurchase(newCost);

            // reroll missions
            for (int i = 0; i < 3; i++)
                RerollMission(i);

            // update variables
            rerollCount++;


        }

        //update cost text in UI
        UpdateRerollText();
    }

    private void UpdateRerollText()
    {
        int cost = ((rerollCount) * rerollCost);
        string costString = cost != 0 ? cost.ToString() : "FREE";
        rerollCostText.text = costString;
    }

    internal void ResetMissions()
    {
        rerollCount = 1;
        availableMissions.Clear();
        for (int i = 0; i < 3; i++)
            currentMissions[i] = null;

        InitialiseMissions();

        //update cost text in UI
        UpdateRerollText();
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

    public void HandleEndRaceEvents()
    {
        double raceTime = MyGameManager.instance.GetRaceManager().GetRaceTime();
        onRaceEndTime.Raise(this, raceTime);
        onRaceEndCarInfo.Raise(MyGameManager.instance.GetCarInfoSerach(), null);
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
        //List<string> eligibleMissions = new() { "Race1", "Race4", "Race5", "Race6" };

        //for (int i = 0; i < 3; i++)
        //{
        //    if (currentMissions[i] != null)
        //    {
        //        foreach(string missionName in eligibleMissions)
        //        {
        //            if (currentMissions[i].title == missionName)
        //            {
        //                if (sender != null)
        //                {
        //                    CarInfoSearch carInfoSerach = sender as CarInfoSearch;
        //                    if (carInfoSerach != null)
        //                    {
        //                        currentMissions[i].carInfoSerach = carInfoSerach;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
    #endregion
    #endregion
}
