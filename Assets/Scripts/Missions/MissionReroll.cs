using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionReroll : MonoBehaviour
{
    private MissionManager missionManager;

    private void Update()
    {
        if(missionManager == null)
            missionManager=FindObjectOfType<MissionManager>();
    }

    public void RerollMission(int index)
    {
        if(missionManager != null)
            missionManager.RerollMission(index);
    }
}
