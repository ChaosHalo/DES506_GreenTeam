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

    [SerializeField]
    protected List<Button> buttonsList;

    [SerializeField]
    protected List<GameObject> backgroundList;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        //buttonsList = FindObjectsOfTypeAll(Button);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log(buttonsList.Count);
        }
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

    }

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
    #endregion
}
