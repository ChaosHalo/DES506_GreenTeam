using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceCamera : MonoBehaviour
{
    private float cameraSwitchCooldown = 0.2f;
    private bool isSwitchCameraOnCooldown = false;
    private CarManager currentTrackedCarManager;
    public RaceCameraScripitObject RaceCameraScripitObject;

    public enum Type { ACTION, FOCUS };
    public Type cameraType;

    [SerializeField] private List<CarManager> availableCarManagers = new();

    [SerializeField] private GameObject cameraFocus;
    [SerializeField] private GameObject cameraCurrent;

    [SerializeField] private Image cameraToggleSpriteRenderer;
    [SerializeField] private Sprite zoomedOutIcon;
    [SerializeField] private Sprite zoomedInIcon;

    private void Update()
    {
        //if (cameraType == Type.ACTION)
        {
            if (cameraCurrent == null)
                if (availableCarManagers.Count > 0)
                    cameraCurrent = availableCarManagers[0].cameraAction;
            UpdateEligibleDCrivers();
        }
    }

    internal void StoreCarManagers()
    {
        // capture and store all car managers in scene
        availableCarManagers.Clear();
        availableCarManagers.AddRange(FindObjectsOfType<CarManager>());

        // setup current tracked car manager
        if (currentTrackedCarManager == null)
            if (availableCarManagers.Count > 0)
                currentTrackedCarManager = availableCarManagers[0];
    }

    private void UpdateEligibleDCrivers()
    {
        // remove cars which have finished race
        if (availableCarManagers.Count > 0)
            availableCarManagers.RemoveAll(item => item.HasFinishedRace());

        // if current tracked car has just finished race, switch to another eligible car
        if (currentTrackedCarManager != null)
            if (availableCarManagers.Count > 0)
                if (currentTrackedCarManager.HasFinishedRace())
                {
                    if (cameraType == Type.ACTION)
                    {
                        FocusOnCar_Action(availableCarManagers[Random.Range(0, availableCarManagers.Count)]);
                    }
                    else if (cameraType == Type.FOCUS)
                    {
                        currentTrackedCarManager = availableCarManagers[0];
                        RaceCameraManager.SetTarget(cameraCurrent, currentTrackedCarManager.transform);
                    }
                }
    }

    // switch to index camera preset in database
    public void SwitchCameraPreset(int index)
    {
        RaceCameraManager.SwitchCamera(cameraCurrent, RaceCameraScripitObject.cameraDatas[index].FollowOffset, RaceCameraScripitObject.cameraDatas[index].fov);
    }

    private void FocusOnCar(string name)
    {
        if (cameraType == Type.FOCUS)
        {
            foreach (var carManager in availableCarManagers)
            {
                if (carManager.CarInfo.Name == name)
                {
                    currentTrackedCarManager = carManager;
                    RaceCameraManager.SetTarget(cameraCurrent, carManager.transform);
                    return;
                }
            }
        }
        else if (cameraType == Type.ACTION)
        {
            foreach (var carManager in availableCarManagers)
            {
                if (carManager.CarInfo.Name == name)
                {
                    FocusOnCar_Action(carManager, true);
                    return;
                }
            }
        }
    }
    public void FocusOnCar_Action(CarManager car, bool overrideCooldown=false)
    {
        // camera switch is on cooldown
        if (isSwitchCameraOnCooldown && overrideCooldown==false)
            return;

        // do not switch to the same camera as current
        if (car.cameraAction == cameraCurrent)
            return;

        // only switch if further than minimum distance
        if (overrideCooldown == false)
            if (Vector3.Distance(car.gameObject.transform.position, currentTrackedCarManager.transform.position) < 30)
                return;

        // switch camera to target and start cooldown
        PerformActionFocus(car);
    }

    private void FocusOnRandomCar_Action()
    {
        // get all available cars
        List<CarManager> allCars = GetCurrentAvailableCars();

        // no cars available, return
        if (allCars.Count < 1)
            return;

        // focus on random new car
        CarManager newCar = allCars[Random.Range(0, allCars.Count)];
        Debug.Log("Focusing: " + newCar.CarInfo.Name);
        FocusOnCar_Action(newCar, true);
    }

    private List<CarManager> GetCurrentAvailableCars()
    {
        List<CarManager> allCars = new();
        List<CarManager> finalCars = new();
        allCars.AddRange(FindObjectsOfType<CarManager>());
      //  allCars.RemoveAll(item => item.HasFinishedRace());
       // allCars.RemoveAll(item => item.cameraAction == cameraCurrent);
       foreach (CarManager carManager in allCars)
            if(carManager.HasFinishedRace() == false && carManager.cameraAction!=cameraCurrent)
                finalCars.Add(carManager);
        return finalCars;
    }

    private void PerformActionFocus(CarManager car)
    {
        currentTrackedCarManager = car;
        CycleActionCamera(car.cameraAction);
        StartCoroutine(StartSwitchTargetCooldown());
    }

    private IEnumerator StartSwitchTargetCooldown()
    {
        isSwitchCameraOnCooldown = true;
        yield return new WaitForSeconds(cameraSwitchCooldown);
        isSwitchCameraOnCooldown = false;
    }


    private void CycleActionCamera(GameObject target)
    {
        if (cameraType == Type.FOCUS)
            return;

        if (cameraCurrent != null)
        {
            cameraCurrent.SetActive(false);
            cameraCurrent = target;
            cameraCurrent.SetActive(true);
        }
    }

    public void Event_FocusOnCar(Component sender, object data)
    {
        if (cameraType == Type.FOCUS)
            return;

        if (isSwitchCameraOnCooldown)
            return;

        if (sender is CarManager)
        {
            if (data is bool)
            {
                if ((bool)data == false)
                {
                    FocusOnCar_Action((CarManager)sender);
                }
            }
        }
    }
    public void Event_OnCarExplode(Component sender, object data)
    {
        if (cameraType == Type.FOCUS)
            return;

        if (sender is CarManager)
        {
            CarManager senderCar = (CarManager)sender;

            // only change camera if current camera is on car that exploded
            if (senderCar.cameraAction != cameraCurrent)
                return;

            List<CarManager> otherCars = new();
            otherCars.AddRange(availableCarManagers);

            if (otherCars.Count > 1)
            {
                otherCars.Remove(senderCar);
                FocusOnCar_Action(otherCars[Random.Range(0, otherCars.Count)]);
            }
        }
    }

    #region EditorStuff
    // editor function
    public void SwitchTarget_Editor(string name) => FocusOnCar(name);
    public void CameraAction(bool pickRandom)
    {
        if (availableCarManagers.Count <= 0)
            return;

        List<CarManager> allCars = GetCurrentAvailableCars();
        if (allCars.Count < 1)
            return;

        cameraToggleSpriteRenderer.sprite = zoomedOutIcon;

        if(cameraType == Type.FOCUS)
        cameraCurrent.SetActive(false);

        cameraType = Type.ACTION;

        if(pickRandom)
        FocusOnRandomCar_Action();
        // StartCoroutine(StartSwitchTargetCooldown());
        //cameraCurrent.SetActive(true);
        // SwitchCameraPreset(0);
    }

    public void ToggleCameraType()
    {
        switch (cameraType)
        {
            case Type.FOCUS:
                {
                    CameraAction(false);
                    PerformActionFocus(currentTrackedCarManager);
                }
                break;
            case Type.ACTION:
                {
                    CameraFocus();
                    FocusOnCar(currentTrackedCarManager.CarInfo.Name);
                }
                break;
        }
    }

    public void Editor_ForceAction()
    {
        CameraAction(false);
        PerformActionFocus(currentTrackedCarManager);
    }

    public void CameraFocus()
    {
        cameraToggleSpriteRenderer.sprite = zoomedInIcon;
        cameraType = Type.FOCUS;
        cameraCurrent.SetActive(false);
        cameraCurrent = cameraFocus;
        cameraCurrent.SetActive(true);
       // SwitchCameraPreset(1);
    }
    #endregion
}
