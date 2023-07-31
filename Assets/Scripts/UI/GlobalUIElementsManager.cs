using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.HighroadEngine;

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

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

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
}
