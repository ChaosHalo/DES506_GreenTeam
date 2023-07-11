using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Build : IGameState
{
    private GameObject buildObjects = null;
    SaveMap saveMap;

    public State_Build() { }

    public void StartState()
    {
        // enable current state objects
        buildObjects = MyGameManager.instance.GetSceneManager().buildObjects;
        if (buildObjects != null)
            buildObjects.SetActive(true);

        // assign refs
        saveMap = MyGameManager.instance.GetSaveMap();

        //MyGameManager.instance.GetMissionManager().CheckForCompletedMissions();
        //MyGameManager.instance.GetCameraManager().ResetCamera();
        MyGameManager.instance.GetObjectPlacer().TriggerFaillAnimations();
    }
    public void EndState()
    {
        saveMap.OnSaveMap();

        if (buildObjects != null)
            buildObjects.SetActive(false);
    }
    public void OnAction()
    {
    }
    public void UpdateState()
    {
    }
}
