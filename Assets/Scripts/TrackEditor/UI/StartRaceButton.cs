using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartRaceButton : MonoBehaviour
{
    public GameObject trackBuilderScreen;
    public GameObject trackBuilderUI;
    public GameObject raceResultsUI;
    public GameObject upcomingRacersScreen;
    public GameStateManager gameStateManager;
    public Button trackButton;

    public void OnClick()
    {
        bool canProceed =  gameStateManager.ChangeToState_Race();
        if (canProceed == false) return;

        trackBuilderScreen.SetActive(false);
        trackBuilderUI.SetActive(false);
        raceResultsUI.SetActive(false);
        upcomingRacersScreen.SetActive(true);
        trackButton.onClick?.Invoke();
    }
}
