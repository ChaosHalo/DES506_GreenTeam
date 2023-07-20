using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class RaceScreenUIManager : MonoBehaviour
{
    public GameObject TimerTextComponent;
    public bool ResetTimeWhenStop = true;
    private TextMeshProUGUI timerText;
    private float timer;
    private bool runTimerFlag;
    private RaceManager raceManager => MyGameManager.instance.GetRaceManager();
    public RaceCameraScripitObject RaceCameraScripitObject;
    public List<Button> RacerInfos = new List<Button>();
    public List<Button> CameraTrackers = new List<Button>();
    public Text ScoreText2;
    public List<TextMeshProUGUI> RankNames = new();
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
        UpdateRank();
    }
    public void UpdateRank()
    {
        // 去除空格
        string rank = ScoreText2.text.Trim();
        string[] ranks = rank.Split("|");
        // 去除最后那个数字
        for (int i = 0; i < ranks.Length; i++)
        {
            ranks[i] = ranks[i].Substring(0, ranks[i].Length - 1);
        }
        foreach (var i in rank)
        {
            Debug.Log(i);
        }
        
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
            CameraTrackers[i].onClick.AddListener(() => SwitchCamera(i));
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
            RacerInfos[i].onClick.AddListener(() => SwitchTarget(carManagers[i].CarInfo.Name));
        }
    }
    public void SwitchCamera(int index)
    {
        //Debug.Log(index);
        RaceCameraManager.SwitchCamera(MyGameManager.instance.RaceCamera, RaceCameraScripitObject.cameraDatas[index].FollowOffset);
    }
    public void SwitchTarget(string name)
    {
        CarManager[] carManagers = FindObjectsOfType<CarManager>();
        foreach(var carManager in carManagers)
        {
            if(carManager.CarInfo.Name == name)
            {
                RaceCameraManager.SetTarget(MyGameManager.instance.RaceCamera, carManager.transform);
                return;
            }
        }
    }
}
