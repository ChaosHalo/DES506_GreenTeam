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

    private CarInfoScriptableObject curCar;
    private int carIndex;
    private void Start()
    {
        curCar = carInfoScriptableObjects[carIndex];
        UpdateCarInfo();
    }

    public void UpdateCarInfo()
    {
        CarName.GetComponent<TextMeshProUGUI>().text = curCar.CarInfo.Name;
        TopSpeedSlider.value = curCar.CarInfo.TopSpeed;
        AccelerationSlider.value = curCar.CarInfo.Acceleration;
        HandlingSlider.value = curCar.CarInfo.Handling;
        OffroadSlider.value = curCar.CarInfo.OffRoad;
        PlaceholderCarImage.sprite = curCar.CarUIInfo.PlaceholderCarImage;
        BackGroundImage.sprite = curCar.CarUIInfo.BackGroundImage;
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
