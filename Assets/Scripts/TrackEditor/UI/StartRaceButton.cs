using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRaceButton : MonoBehaviour
{
    public GameObject trackBuilderScreen;
    public GameObject trackBuilderUI;
    public GameObject upcomingRacersScreen;
    public GameStateManager gameStateManager;

    public void OnClick()
    {
        bool canProceed =  gameStateManager.ChangeToState_Race();
        if (canProceed == false) return;

        trackBuilderScreen.SetActive(false);
        trackBuilderUI.SetActive(false);
        upcomingRacersScreen.SetActive(true);
    }
}
