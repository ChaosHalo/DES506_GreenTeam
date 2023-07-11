using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using TMPro;
public class RaceScreenUIManager : MonoBehaviour
{
    public GameObject TimerTextComponent;
    public bool ResetTimeWhenStop = true;
    private TextMeshProUGUI timerText;
    private float timer;
    private bool runTimerFlag;
    private RaceManager raceManager => MyGameManager.instance.GetRaceManager();
    // Start is called before the first frame update
    void Start()
    {
        timerText = TimerTextComponent.GetComponent<TextMeshProUGUI>();

        if (raceManager != null)
        {
            raceManager.StartRaceEvent.AddListener(StartTimer);
            raceManager.EndRaceEvent.AddListener(StopTimer);
        }
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
}
