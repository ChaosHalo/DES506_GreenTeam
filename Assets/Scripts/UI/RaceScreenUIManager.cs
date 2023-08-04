using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using Cinemachine;
public class RaceScreenUIManager : MonoBehaviour
{
    public GameObject TimerTextComponent;
    public bool ResetTimeWhenStop = true;
    private TextMeshProUGUI timerText;
    private float timer;
    private bool runTimerFlag;

    private CinemachineBrain cinemachineBrain;
    private RaceManager raceManager => MyGameManager.instance.GetRaceManager();
    private CarManager[] carManagers;
    private Dictionary<string, GameObject> CompletedRaceIconDics = new();
    public CarInfoScriptableObject[] CarInfoScriptableObjects;
    public RaceCameraScripitObject RaceCameraScripitObject;
    public List<Button> RacerInfos = new List<Button>();
    public List<Button> CameraTrackers = new List<Button>();
    public Text ScoreText2;
    public List<GameObject> Ranks = new();
    public List<GameObject> CompletedRaceIcons = new();
    public Color FinishColor;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke(nameof(StartTimer), raceManager.StartGameCountDownTime - 1);
        if (raceManager != null)
        {
            //raceManager.StartRaceEvent.AddListener(StartTimer);
            raceManager.EndRaceEvent.AddListener(StopTimer);
        }
        carManagers = FindObjectsOfType<CarManager>();
        InitCompletedRaceIcon();
        InitFocusIcons();
    }
    void Start()
    {
        timerText = TimerTextComponent.GetComponent<TextMeshProUGUI>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();


        /*InitRacerInfos();
        InitCameraTracker();*/
    }
    private void OnDisable()
    {
        if (raceManager != null)
        {
            //raceManager.StartRaceEvent.RemoveListener(StartTimer);
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
        UpdateFinishFlagIcon();
    }
    private void InitCompletedRaceIcon()
    {
        CompletedRaceIconDics.Clear();
        CompletedRaceIconDics.Add(GlobalConstants.BILLY, CompletedRaceIcons[0]);
        CompletedRaceIconDics.Add(GlobalConstants.PETER, CompletedRaceIcons[1]);
        CompletedRaceIconDics.Add(GlobalConstants.MIK, CompletedRaceIcons[2]);
        CompletedRaceIconDics.Add(GlobalConstants.FELICIA, CompletedRaceIcons[3]);

        foreach (var i in CompletedRaceIcons)
        {
            Image completedRaceIcon = i.GetComponentsInChildren<Image>()[1];
            completedRaceIcon.enabled = false;
        }
    }
    private void InitFocusIcons()
    {
        foreach (var rank in Ranks)
        {
            Image focusImage = rank.GetComponentsInChildren<Image>()[1];
            focusImage.enabled = false;
        }
    }
    private void Init() { }
    // yellow / red / green / blue
    private void UpdateFinishFlagIcon()
    {
        foreach (var car in carManagers)
        {
            if (car.HasFinishedRace())
            {
                Image[] images = CompletedRaceIconDics[car.CarInfo.Name].GetComponentsInChildren<Image>();
                images[0].color = FinishColor;
                images[1].enabled = true;
            }
        }
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
            Image focusImage = Ranks[i].GetComponentsInChildren<Image>()[1];
            TextMeshProUGUI rankName = Ranks[i].GetComponentInChildren<TextMeshProUGUI>();

            if (carRankImages.ContainsKey(carName))
            {
                rankImage.sprite = carRankImages[carName];
                rankName.text = carName;

                // 更新focus图标，表示摄像机当前正在看着的车
                CinemachineVirtualCamera activeVirtualCamera = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

                if (activeVirtualCamera != null && activeVirtualCamera.Follow != null)
                {
                    string targetName = activeVirtualCamera.Follow.gameObject.GetComponent<CarManager>().CarInfo.Name;
                    //Debug.Log(targetName);
                    if (targetName == carName)
                        focusImage.enabled = true;
                    else
                        focusImage.enabled = false;
                }
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
        if (runTimerFlag)
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
#endregion
//*private void InitCameraTracker()
//{
//    for (int i = 0; i < CameraTrackers.Count; i++)
//    {
//        //buttons[i].onClick.RemoveAllListeners();
//       // CameraTrackers[i].onClick.AddListener(() => SwitchCamera(i));
//    }
//}
//private void InitRacerInfos()
//{
//    CarManager[] carManagers = FindObjectsOfType<CarManager>();
//    for(int i = 0; i < RacerInfos.Count; i++)
//    {
//        Debug.Log(i + "InitRacerInfos");
//        RacerInfos[i].GetComponentInChildren<TextMeshProUGUI>().text = carManagers[i].CarInfo.Name;
//        //RacerInfos[i].GetComponent<Button>().onClick.RemoveAllListeners();
//      //  RacerInfos[i].onClick.AddListener(() => SwitchTarget(carManagers[i].CarInfo.Name));
//    }
//}
//public void SwitchCamera(int index)
//{
//    //Debug.Log(index);
//    RaceCameraManager.SwitchCamera(MyGameManager.instance.RaceCamera, RaceCameraScripitObject.cameraDatas[index].FollowOffset);
//}
//public void SwitchTarget(string name)
//{
//    CarManager[] carManagers = FindObjectsOfType<CarManager>();
//    foreach(var carManager in carManagers)
//    {
//        if(carManager.CarInfo.Name == name)
//        {
//            RaceCameraManager.SetTarget(MyGameManager.instance.RaceCamera, carManager.transform);
//            return;
//        }
//    }
//}
