using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.HighroadEngine;
public class UpcomingUIManager : MonoBehaviour
{
    public List<CarInfoScriptableObject> carInfoScriptableObjects;
    public GameObject CarName;
    public Image racerIcon;

    public Sprite[] allIcons;

    public Slider TopSpeedSlider;
    public Slider AccelerationSlider;
    public Slider HandlingSlider;
    public Slider OffroadSlider;

    public Image PlaceholderCarImage;
    public Image BackGroundImage;

    public GameObject InfoText;
    public TextMeshProUGUI RaceCounter;

    [Header("Music")]
    public Sprite MusicOn;
    public Sprite MusicOff;
    public Image MusicIcon;

    private CarInfoScriptableObject curCar;
    private int carIndex;

    private SoundManager soundManager;
    private void OnEnable()
    {
        curCar = carInfoScriptableObjects[carIndex];
        UpdateCarInfo();
    }
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    private CarInfo GetCarInfo(CarInfoScriptableObject carInfoScriptableObject)
    {
        int seasonIndex = MyGameManager.instance.GameRound % MyGameManager.instance.Season;
        int index = Mathf.Min(carInfoScriptableObject.CarInfos.Count - 1, seasonIndex);
        return carInfoScriptableObject.GetCarInfo(index);
    }
    public void UpdateCarInfo()
    {
        racerIcon.sprite = allIcons[carIndex];
        CarName.GetComponent<TextMeshProUGUI>().text = GetCarInfo(curCar).Name;
        TopSpeedSlider.value = GetCarInfo(curCar).TopSpeed;
        AccelerationSlider.value = GetCarInfo(curCar).Acceleration;
        HandlingSlider.value = GetCarInfo(curCar).Handling;
        OffroadSlider.value = GetCarInfo(curCar).OffRoad;
        PlaceholderCarImage.sprite = curCar.CarUIInfo.PlaceholderCarImage;
        BackGroundImage.sprite = curCar.CarUIInfo.BackGroundImage;
        InfoText.GetComponent<TextMeshProUGUI>().text = curCar.CarUIInfo.Info;
        RaceCounter.text = ((MyGameManager.instance.GameRound % MyGameManager.instance.Season) + 1).ToString() + " / " + MyGameManager.instance.Season;
        // RaceCounter.text = (MyGameManager.instance.CurSeason + 1).ToString() + " / " + MyGameManager.instance.Season;
    }
    public void NextCar()
    {
        carIndex++;
        if(carIndex >= carInfoScriptableObjects.Count)
        {
            carIndex = 0;
        }
        curCar = carInfoScriptableObjects[carIndex];
        UpdateCarInfo();
    }
    public void PreviousCar()
    {
        carIndex--;
        if (carIndex < 0)
        {
            carIndex = carInfoScriptableObjects.Count - 1;
        }
        curCar = carInfoScriptableObjects[carIndex];
        UpdateCarInfo();
    }
    public void OnClickMusic()
    {
        soundManager.SwitchMusic();
        if (soundManager.IsMusicPlaying()) MusicIcon.sprite = MusicOn;
        else MusicIcon.sprite = MusicOff;
    }
}
