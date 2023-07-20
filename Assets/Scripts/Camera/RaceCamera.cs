using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour
{
    private float cameraSwitchCooldown = 1f;
    private float minDistanceBeforeSwitch = 50f;
    private bool isSwitchCameraOnCooldown = false;
    private string currentTrackedDriver = "Felicia";
    private CarManager currentTrackedCarManager;
    public RaceCameraScripitObject RaceCameraScripitObject;

    private List<string> allDriverStrings = new List<string>() { "Felicia", "Mik", "Peter", "Billy" };
    private List<string> availableDriverStrings = new();

    public enum Type { ACTION, FOCUS };
    public Type cameraType;

    [SerializeField] private List<CarManager> allCars = new();

    [SerializeField] private GameObject cameraAction;
    [SerializeField] private GameObject cameraFocus;
    private GameObject currentCameraObject;

    private void Start()
    {
        availableDriverStrings.AddRange(allDriverStrings);
    }

    private void Update()
    {
        if (cameraType == Type.ACTION)
        {
            GetDrivers();
            UpdateEligibleDCrivers();
        }
    }

    internal void GetDrivers()
    {
        allCars.Clear();
        allCars.AddRange(FindObjectsOfType<CarManager>());
        if (currentTrackedCarManager == null)
            if (allCars.Count > 0)
                currentTrackedCarManager = allCars[0];
    }

    private void UpdateEligibleDCrivers()
    {
        if (allCars.Count > 0)
        {
            allCars.RemoveAll(item => item.HasFinishedRace());
            allCars.RemoveAll(item => item.isRespawning);

            // sort by longest air duration
            allCars.Sort(SortByAirDuration);

            // switch to car with longest air duration
            if (allCars.Count > 0)
            {
                CarManager car = allCars[allCars.Count - 1];
                if (car != null)
                {
                    if(Vector3.Distance(car.gameObject.transform.position, currentTrackedCarManager.transform.position)>minDistanceBeforeSwitch)
                    {
                        SwitchTarget(car.CarInfo.Name, false, true);
                    }
                }
            }
        }

    }

    static int SortByAirDuration(CarManager c1, CarManager c2)
    {
        return c1.airDuration.CompareTo(c2.airDuration);
    }

    // switch to index camera preset in database
    public void SwitchCamera(int index)
    {
        RaceCameraManager.SwitchCamera(MyGameManager.instance.RaceCameraObject, RaceCameraScripitObject.cameraDatas[index].FollowOffset, RaceCameraScripitObject.cameraDatas[index].fov);
    }

    // will only zoom if name is the same as current driver
    public void TryZoomOnDriver(string name)
    {
        if (name == currentTrackedDriver)
        {
            StartCoroutine(FocusOnCurrentDriverForDuration(1f));
        }
    }

    // if autoSwitchAfterDelay == TRUE then switch to a different driver after delay
    public void SwitchTarget(string name, bool autoSwitchAfterDelay = false, bool startCooldown = false)
    {
        if (startCooldown && isSwitchCameraOnCooldown)
            return;

        foreach (var carManager in allCars)
        {
            if (carManager.CarInfo.Name == name)
            {
                currentTrackedDriver = name;
                currentTrackedCarManager = carManager;
                RaceCameraManager.SetTarget(MyGameManager.instance.RaceCameraObject, carManager.transform);

                RaceCameraManager.SetFOV(MyGameManager.instance.RaceCameraObject, 25);

                //if (autoSwitchAfterDelay)
                   // StartCoroutine(SwitchToAnotherDriverAfterDelay(2.25f));

                if (startCooldown)
                    StartCoroutine(RunSwitchTargetCooldown());

                return;
            }
        }
    }

    private IEnumerator RunSwitchTargetCooldown()
    {
        isSwitchCameraOnCooldown = true;
        yield return new WaitForSeconds(cameraSwitchCooldown);
        isSwitchCameraOnCooldown = false;
    }

    // zoom in FoV on current driver for duration
    private IEnumerator FocusOnCurrentDriverForDuration(float duration)
    {
        //RaceCameraManager.SetFOV(MyGameManager.instance.RaceCameraObject, 17.5f);
        yield return new WaitForSeconds(duration);
        // RaceCameraManager.SetFOV(MyGameManager.instance.RaceCameraObject, 25);
    }

    // switch to different driver after delay
    internal IEnumerator SwitchToAnotherDriverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        availableDriverStrings.Clear();
        availableDriverStrings.AddRange(allDriverStrings);
        availableDriverStrings.Remove(currentTrackedDriver);
        SwitchTarget(availableDriverStrings[Random.Range(0, availableDriverStrings.Count)], false, true);
    }


    #region EditorStuff
    // editor function
    public void SwitchTarget_Editor(string name) => SwitchTarget(name, false);
    public void CameraAction()
    {
        cameraType = Type.ACTION;
        cameraAction.SetActive(true);
        cameraFocus.SetActive(false);
        currentCameraObject = cameraAction;
        MyGameManager.instance.RaceCameraObject = currentCameraObject;
        SwitchCamera(0);
    }

    public void CameraFocus()
    {
        cameraType = Type.FOCUS;
        cameraAction.SetActive(false);
        cameraFocus.SetActive(true);
        currentCameraObject = cameraFocus;
        MyGameManager.instance.RaceCameraObject = currentCameraObject;
        SwitchCamera(1);
    }
    #endregion
}
