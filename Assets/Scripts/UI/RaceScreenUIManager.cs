using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class RaceScreenUIManager : MonoBehaviour
{
    public GameObject TimerTextComponent;
    public bool ResetTimeWhenStop = true;
    private TextMeshProUGUI timerText;
    private float timer;
    private bool runTimerFlag;
    private RaceManager raceManager => MyGameManager.instance.GetRaceManager();
    public CarInfoScriptableObject[] CarInfoScriptableObjects;
    public RaceCameraScripitObject RaceCameraScripitObject;
    public List<Button> RacerInfos = new List<Button>();
    public List<Button> CameraTrackers = new List<Button>();
    public Text ScoreText2;
    public List<GameObject> Ranks = new();
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
        string[] rankNames = ParsePlayers(ScoreText2.text);
        Dictionary<string, Sprite> carRankImages = new();
        foreach (var i in CarInfoScriptableObjects)
        {
            carRankImages.Add(i.GetCarInfo(0).Name, i.CarUIInfo.RankImage);
        }
        for (int i = 0; i < carRankImages.Count; i++)
        {
            string carName = rankNames[i];
            if (carName == null || carName.Length == 0) return;
            Image rankImage = Ranks[i].GetComponent<Image>();
            TextMeshProUGUI rankName = Ranks[i].GetComponentInChildren<TextMeshProUGUI>();

            if (carRankImages.ContainsKey(carName))
            {
                rankImage.sprite = carRankImages[carName];
                rankName.text = carName;
            }
            else
            {
                // Handle the case when the carName is not found in the dictionary
                Debug.LogError($"Car name '{carName}' not found in carRankImages dictionary.");
            }

        }
    }
    string[] ParsePlayers(string inputText)
    {
        // 按行分隔字符串
        string[] lines = inputText.Split('\n');

        // 初始化玩家数组
        string[] playerArray = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            // 清除每行前后的空白字符
            string playerName = lines[i].Trim();

            // 移除每行中的"|"
            playerName = playerName.Replace("| ", "");

            // 移除名字后面的数字
            playerName = RemoveNumbers(playerName);

            // 存储每个玩家名字到数组中
            if (playerName == "") continue;
            playerArray[i] = playerName;
            //Debug.Log(playerName);
        }
        return playerArray;
    }

    // 移除名字后面的数字
    string RemoveNumbers(string input)
    {
        return new string(input.Where(c => !char.IsDigit(c)).ToArray());
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
    /*private void InitCameraTracker()
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
    }*/
    /*public void SwitchCamera(int index)
    {
        //Debug.Log(index);
        RaceCameraManager.SwitchCamera(MyGameManager.instance.raceCamera, RaceCameraScripitObject.cameraDatas[index].FollowOffset);
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
    }*/
}
