using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera: MonoBehaviour
{
    private float cameraSwitchCooldown = 1f;
    private bool isSwitchCameraOnCooldown = false;
    private string currentTrackedDriver = "Felicia";
    public RaceCameraScripitObject RaceCameraScripitObject;

    private List<string> allDrivers = new List<string>() { "Felicia", "Mik", "Peter", "Billy" };
    private List<string> availableDrivers=new();
    CarManager[] carManagers;

    private void Start()
    {
        availableDrivers.AddRange(allDrivers);
    }

    private void Update()
    {
        AutoSwitchCamera();
    }

    private void AutoSwitchCamera()
    {
        if (isSwitchCameraOnCooldown)
            return;

        // all cars
        carManagers = FindObjectsOfType<CarManager>();
        List<CarManager> allCars = new();

        // only check cars which are not respawning
        foreach (CarManager carManager in carManagers)
            if (carManager.isRespawning == false)
                allCars.Add(carManager);

        // sort by longest air duration
        allCars.Sort(SortByAirDuration);

        // switch to car with longest air duration
        if (allCars.Count > 0)
            if (allCars[allCars.Count - 1] != null)
                SwitchTarget(allCars[allCars.Count - 1].CarInfo.Name, false, true);
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
    public void SwitchTarget(string name, bool autoSwitchAfterDelay = false, bool startCooldown=false)
    {
        if (startCooldown && isSwitchCameraOnCooldown)
            return;

        foreach (var carManager in carManagers)
        {
            if (carManager.CarInfo.Name == name)
            {
                currentTrackedDriver = name;
                RaceCameraManager.SetTarget(MyGameManager.instance.RaceCameraObject, carManager.transform);

                RaceCameraManager.SetFOV(MyGameManager.instance.RaceCameraObject, 25);
                
                if(autoSwitchAfterDelay)
                    StartCoroutine(SwitchToAnotherDriverAfterDelay(2.25f));

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
        availableDrivers.Clear();
        availableDrivers.AddRange(allDrivers);
        availableDrivers.Remove(currentTrackedDriver);
        SwitchTarget(availableDrivers[Random.Range(0, availableDrivers.Count)], false, true);
    }


    #region EditorStuff
    // editor function
    public void SwitchTarget_Editor(string name)
    {
        SwitchTarget(name, false);
    }
    #endregion
}
