using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpcomingUIManager : MonoBehaviour
{
    public List<CarInfoScriptableObject> carInfoScriptableObjects;
    public GameObject CarName;

    public Slider TopSpeedSlider;
    public Slider AccelerationSlider;
    public Slider HandlingSlider;
    public Slider OffroadSlider;

    public Image PlaceholderCarImage;
    public Image BackGroundImage;

    public GameObject InfoText;

    private CarInfoScriptableObject curCar;
    private int carIndex;
    private void OnEnable()
    {
        curCar = carInfoScriptableObjects[carIndex];
        UpdateCarInfo();
    }
    private void Start()
    {
        
    }
    private CarInfo GetCarInfo(CarInfoScriptableObject carInfoScriptableObject)
    {
        int index = Mathf.Min(carInfoScriptableObject.CarInfos.Count - 1, MyGameManager.instance.CurSeason);
        return carInfoScriptableObject.GetCarInfo(index);
    }
    public void UpdateCarInfo()
    {
        CarName.GetComponent<TextMeshProUGUI>().text = GetCarInfo(curCar).Name;
        TopSpeedSlider.value = GetCarInfo(curCar).TopSpeed;
        AccelerationSlider.value = GetCarInfo(curCar).Acceleration;
        HandlingSlider.value = GetCarInfo(curCar).Handling;
        OffroadSlider.value = GetCarInfo(curCar).OffRoad;
        PlaceholderCarImage.sprite = curCar.CarUIInfo.PlaceholderCarImage;
        BackGroundImage.sprite = curCar.CarUIInfo.BackGroundImage;
        InfoText.GetComponent<TextMeshProUGUI>().text = curCar.CarUIInfo.Info;
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
}
