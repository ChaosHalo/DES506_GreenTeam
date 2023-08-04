using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.HighroadEngine;

public class StartRaceButton : MonoBehaviour
{
    public GameObject trackBuilderScreen;
    public GameObject trackBuilderUI;
    public GameObject raceResultsUI;
    public GameObject upcomingRacersScreen;
    public GameStateManager gameStateManager;
    public Button trackButton;
    public RaceCamera raceCamera;

    [SerializeField]
    public GameObject tutorialUIFinishTrack;

    [SerializeField]
    public GameObject tutorialUIRaceScreen;

    [SerializeField]
    public RaceManager raceManager;

    public void OnClick()
    {
        // check if world is still generating
        if (MyGameManager.instance.GetPlacementSystem().buildingState != null)
        {
            if (MyGameManager.instance.GetPlacementSystem().buildingState.GetType() == typeof(State_GenerateWorld))
            {
                Debug.Log("World currently generating");
                return;
            }
        }

        // check for track drop/delete/rotate animations
        if (MyGameManager.instance.GetObjectPlacer().IsTrackAnimating() == true)
        {
            Debug.Log("Objects currently animating");
            return;
        }

        // begin chain check if each piece is connected to the start
        MyGameManager.instance.GetObjectPlacer().StartChainCheck();

        // check if each piece is individually connected
        if (MyGameManager.instance.GetObjectPlacer().IsTrackFullyConnected() == false)
        {
            return;
        }

        // can proceed to race
        //trackButton.onClick.Invoke();
        trackBuilderScreen.SetActive(false);
        trackBuilderUI.SetActive(false);
        raceResultsUI.SetActive(false);
        upcomingRacersScreen.SetActive(true);
        //trackButton.onClick?.Invoke();
        gameStateManager.ChangeToState_Race();

        if (tutorialUIFinishTrack.activeSelf == true)
        {
            raceManager.tutorialCanStartRace = false;
            tutorialUIFinishTrack.SetActive(false);
            tutorialUIRaceScreen.SetActive(true);
        }

        raceCamera.Editor_ForceAction();
    }
}
