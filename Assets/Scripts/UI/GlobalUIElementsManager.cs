using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.HighroadEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GlobalUIElementsManager : MonoBehaviour
{
    [Header("Music")]
    public Sprite MusicOn;
    public Sprite MusicOff;
    public Image MusicIcon;

    [Header("Sound")]
    public Sprite SoundOn;
    public Sprite SoundOff;
    public Image SoundIcon;

    private SoundManager soundManager;
    private MissionManager missionManager;
    [SerializeField]
    private RaceManager raceManager;

    [SerializeField]
    protected GameObject tutorialPopUps;
    /*
    [SerializeField]
    protected GameObject restartTutorialButton;
    */
    [SerializeField]
    protected List<Button> buttonsList;

    [SerializeField]
    protected List<GameObject> backgroundList;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        missionManager = FindObjectOfType<MissionManager>();
        //raceManager = FindObjectOfType<RaceManager>();

        //buttonsList = FindObjectsOfTypeAll(Button);
    }

    #region Music & Sound
    public void OnClickMusic()
    {
        if (soundManager.IsMusicPlaying()) TurnOffMusic();
        else TurnOnMusic();
    }
    public void TurnOnMusic()
    {
        soundManager.TurnOnMusic();
        MusicIcon.sprite = MusicOn;
    }
    public void TurnOffMusic()
    {
        soundManager.TurnOffMusic();
        MusicIcon.sprite = MusicOff;
    }
    public void OnClickSFX()
    {
        if (soundManager.IsSFXPlaying()) TurOffSFX();
        else TurnOnSFX();
    }
    public void TurnOnSFX()
    {
        soundManager.TurnOnSFX();
        SoundIcon.sprite = SoundOn;
    }
    public void TurOffSFX()
    {
        soundManager.TurnOffSFX();
        SoundIcon.sprite = SoundOff;
    }
    #endregion

    #region Tutorial
    public void SkipTutorial()
    {
        EnableAllButtons();
        DisableAllBackgrounds();

        //If mission re-roll = 0, Set re-roll cost to 100
        if (missionManager.rerollCount == 0)
        {
            missionManager.rerollCount = 1;
            missionManager.rerollCostText.text = ((missionManager.rerollCount) * missionManager.rerollCost).ToString();
        }

        //Unlock curve piece (done in inspector)


        tutorialPopUps.SetActive(false);
        //restartTutorialButton.SetActive(true);
    }
    /*
    public void RestartTutorial()
    {
        DisableAllButtons();
        backgroundList[0].SetActive(true);

        tutorialPopUps.SetActive(true);
        restartTutorialButton.SetActive(false);
    }
    */
    public void EnableAllButtons()
    {
        foreach (Button button in buttonsList)
        {
            button.enabled = true;
        }
    }

    public void DisableAllButtons()
    {
        foreach (Button button in buttonsList)
        {
            button.enabled = false;
        }
    }

    public void DisableAllBackgrounds()
    {
        foreach (GameObject background in backgroundList)
        {
            background.SetActive(false);
        }
    }

    public void DisableStartRace()
    {
        raceManager.tutorialCanStartRace = false;
    }

    public void EnableStartRace()
    {
        raceManager.tutorialCanStartRace = true;
    }

    public void TutorialStartRace()
    {
        EnableStartRace();
        raceManager.ManagerStart();
    }
    #endregion
}
