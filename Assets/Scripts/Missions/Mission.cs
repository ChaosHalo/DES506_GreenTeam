using System;
using System.Linq;
using UnityEditor.SceneManagement;
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
    #endregion

    #region GOAL
    [Header("GOAL")]
    internal int var1;
    [Header("Integer Variable 1")]
    public int var1_min;
    public int var1_max;

    internal int var2;
    [Header("Integer Variable 2")]
    public int var2_min;
    public int var2_max;

    internal float var3;
    [Header("Float Variable 1")]
    public float var3_min;
    public float var3_max;

    internal int varExtra1=0;
    internal int varExtra2=0;
    internal int varExtra3=0;
    internal int varExtra4=0;
    internal int varExtra5=0;

    [Header("Goal Track Piece Type")]
    public ObjectData.TrackType trackType;

    [Header("Goal Terrain Piece Type")]
    public ObjectData.TerrainType terrainType;

    internal int goalInt;
    internal float goalFloat;
    #endregion

    public virtual void InitialiseMission()
    {
        // randomise variables
        var1 = UnityEngine.Random.Range(var1_min, var1_max+1);
        var2 = UnityEngine.Random.Range(var2_min, var2_max+1);
        var3 = UnityEngine.Random.Range(var3_min, var3_max);

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
    public virtual bool IsReached() { return false; }
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
