using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Mission : ScriptableObject
{
    #region MISSION
    [Header("MISSION")]
    internal bool isActive;
    internal int index;

    public string title;
    internal string description;
    internal int rewardCurrency;
    public int grouping;

    public Difficulty missionDifficulty;
    public enum Difficulty { EASY, MEDIUM, HARD }
    public Type missionType;
    public enum Type { NONE, TRACK, RACE }

    internal string colourInProgress = "<color=#798F9D>";
    internal string colourComplete = "<color=#00B729>";
    #endregion

    #region GOAL
    [Header("GOAL")]
    internal int int1;
    [Header("Integer Variable 1")]
    public int int1_min;
    public int int1_max;

    internal int int2;
    [Header("Integer Variable 2")]
    public int int2_min;
    public int int2_max;

    internal int int3;
    [Header("Integer Variable 3")]
    public int int3_min;
    public int int3_max;

    internal float float1;
    [Header("Float Variable 1")]
    public float float1_min;
    public float float1_max;

    internal int varExtra1=0;
    internal int varExtra2=0;
    internal int varExtra3=0;
    internal int varExtra4=0;
    internal int varExtra5=0;

    internal string name1;
    internal string name2;
    List<string> allNames = new List<string>() { "Peter", "Mik", "Felicia", "Billy" };

    [Header("Goal Track Piece Type")]
    public ObjectData.TrackType trackType;

    [Header("Goal Terrain Piece Type")]
    public ObjectData.TerrainType terrainType;

    internal int goalInt;
    internal float goalFloat;
    internal double goalDouble;
    #endregion

    public virtual void InitialiseMission()
    {
        // randomise variables
        int1 = UnityEngine.Random.Range(int1_min, int1_max+1);
        int2 = UnityEngine.Random.Range(int2_min, int2_max+1);
        int3 = UnityEngine.Random.Range(int3_min, int3_max + 1);
        float1 = UnityEngine.Random.Range(float1_min, float1_max);

        // randomise names
        int i = UnityEngine.Random.Range(0, allNames.Count);
        name1 = allNames[i];
        allNames.RemoveAt(i);
        i = UnityEngine.Random.Range(0, allNames.Count);
        name2 = allNames[i];

        // randomise track & terrain
        trackType = (ObjectData.TrackType)UnityEngine.Random.Range(1, (int)Enum.GetValues(typeof(ObjectData.TrackType)).Cast<ObjectData.TrackType>().Max());
        terrainType = (ObjectData.TerrainType)UnityEngine.Random.Range(1, (int)Enum.GetValues(typeof(ObjectData.TerrainType)).Cast<ObjectData.TerrainType>().Max() + 1);

        // set variables
        missionDifficulty = GetDifficulty();
        description = GetDescriptionText();
        rewardCurrency = CalculateReward();
    }

    public int CalculateReward()
    {
        int reward = 0;

        switch (missionDifficulty)
        {
            case Difficulty.EASY:
                reward = 750;
                break;
            case Difficulty.MEDIUM:
                reward = 1000;
                break;
            case Difficulty.HARD:
                reward = 1500;
                break;
        }

        return reward;
    }

    public virtual string GetDescriptionText() { return "NONE"; }
    public virtual bool IsGoalReached() { return false; }

    public virtual string GetProgressString() 
    {
        if(missionType==Type.TRACK)
            return colourInProgress + "<b> (" + (Convert.ToInt32(IsGoalReached())).ToString() + "/1)";
        else
            return colourInProgress+ "<b> (0/1)";
    }
    public virtual Mission.Difficulty GetDifficulty() { return Mission.Difficulty.EASY; }

    public Color GetDifficultyColour()
    {
        Color newCol = Color.green;

        switch(missionDifficulty)
        {
            case Difficulty.EASY:
                newCol = Color.green; 
                break;
            case Difficulty.MEDIUM:
                newCol = Color.yellow;
                break;
            case Difficulty.HARD:
                newCol = Color.red;
                break;
        }

        newCol.a = 0.5f;
        return newCol;
    }
}
