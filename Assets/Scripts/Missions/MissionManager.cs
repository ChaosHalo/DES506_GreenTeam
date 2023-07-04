using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("All Possible Missions")]
    public List<Mission> allMissions = new List<Mission>();

    [Header("Current Missions")]
    public Mission[] currentMissions = new Mission[3];

    [Header("UI")]
    public MissionUI missionUI;

    private void Start()
    {
        InitialiseMissions();
    }

    private void Update()
    {
        if (missionUI == null)
            missionUI = FindObjectOfType<MissionUI>();
    }

    private void InitialiseMissions()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] == null)
            {
                CreateNewMission(i);
            }
        }
    }

    public void CreateNewMission(int index)
    {
        currentMissions[index] = Instantiate(allMissions[Random.Range(0, allMissions.Count)]);
        currentMissions[index].InitialiseMission();
        currentMissions[index].index = index;
    }

    public void RerollMission(int index)
    {
        currentMissions[index] = null;
        CreateNewMission(index);
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
        for (int i = 0; i < 3; i++)
        {
            if (currentMissions[i] != null)
            {
                if (currentMissions[i].title == "Track7" || currentMissions[i].title == "Track8")
                {
                    if(data is int)
                    {
                        currentMissions[i].goalInt += (int)data;
                    }
                }
            }
        }
    }
    #endregion
    #endregion
}
