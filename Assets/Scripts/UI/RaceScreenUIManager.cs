using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using TMPro;
using UnityEngine.UI;
public class RaceScreenUIManager : MonoBehaviour
{
    public GameObject TimerTextComponent;
    public bool ResetTimeWhenStop = true;
    private TextMeshProUGUI timerText;
    private float timer;
    private bool runTimerFlag;
    private RaceManager raceManager => MyGameManager.instance.GetRaceManager();
    public List<Button> RacerInfos = new List<Button>();
    public List<Button> CameraTrackers = new List<Button>();

    [SerializeField] private RaceCamera raceCamera;


    // Start is called before the first frame update
    void Start()
    {
        timerText = TimerTextComponent.GetComponent<TextMeshProUGUI>();

        if (raceManager != null)
        {
            raceManager.StartRaceEvent.AddListener(StartTimer);
            raceManager.EndRaceEvent.AddListener(StopTimer);
        }
        /*InitRacerInfos();
        InitCameraTracker();*/
    }
    private void OnDisable()
    {
        if (raceManager != null)
        {
            raceManager.StartRaceEvent.RemoveListener(StartTimer);
            raceManager.EndRaceEvent.RemoveListener(StopTimer);
        }
        StopTimer();
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTimerText();
        UpdateTimer();
    }
    #region Timer
    private void UpdateTimer()
    {
        if (!runTimerFlag)
        {
            timer += Time.deltaTime;
        }
    }
    public void StartTimer()
    {
        runTimerFlag = true;
    }
    public void StopTimer()
    {
        if (ResetTimeWhenStop) timer = 0;
        runTimerFlag = false;
    }
    public float GetCurTime()
    {
        return timer;
    }
    private void UpdateTimerText()
    {
        timerText.text = GetCurTime().ToString("f3");
    }
    #endregion
    private void InitCameraTracker()
    {
        for (int i = 0; i < CameraTrackers.Count; i++)
        {
            //buttons[i].onClick.RemoveAllListeners();
            CameraTrackers[i].onClick.AddListener(() => raceCamera.SwitchCamera(i));
        }
    }
    private void InitRacerInfos()
    {
        CarManager[] carManagers = FindObjectsOfType<CarManager>();
        for(int i = 0; i < RacerInfos.Count; i++)
        {
            Debug.Log(i + "InitRacerInfos");
            RacerInfos[i].GetComponentInChildren<TextMeshProUGUI>().text = carManagers[i].CarInfo.Name;
            //RacerInfos[i].GetComponent<Button>().onClick.RemoveAllListeners();
            RacerInfos[i].onClick.AddListener(() => raceCamera.SwitchTarget(carManagers[i].CarInfo.Name, false));
        }
    }
}
