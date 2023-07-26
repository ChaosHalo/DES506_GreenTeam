using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class EndOfSeasonUIManager : AllUIManager
{
    public GameObject BestDriverName;
    public GameObject WorstDriverName;
    public GameObject BestDriverTime;
    public GameObject WorstDriverTime;

    private void Start()
    {
        /*UpdateBestDriverName();
        UpdateWorstDriverName();
        UpdateBestDriverTime();
        UpdateWorstDriverTime();*/
    }
    private void OnEnable()
    {
        UpdateMapScreenShots();
        UpdateBestDriverName();
        UpdateWorstDriverName();
        UpdateBestDriverTime();
        UpdateWorstDriverTime();
    }
    #region WorstAndBestDriverHandle
    private List<OneRoundRaceResultData> GetSeasonRoundData()
    {
        List<OneRoundRaceResultData> oneRoundRaceResultDatas = MyGameManager.instance.OneRoundRaceResultDatas;
        return oneRoundRaceResultDatas.GetRange(oneRoundRaceResultDatas.Count - MyGameManager.instance.Season, MyGameManager.instance.Season);
    }
    private OneRoundRaceResultData GetMergeRoundData()
    {
        List<OneRoundRaceResultData> Season = GetSeasonRoundData();
        OneRoundRaceResultData oneRoundRaceResultData = new();
        foreach(var raceData in Season)
        {
            var tempCars = raceData.OneCarRaceResultDatas;
            foreach(var oneCarData in tempCars)
            {
                if (!oneRoundRaceResultData.ContainCar(oneCarData.CarName))
                {
                    oneRoundRaceResultData.AddCarData(oneCarData);
                }
                else
                {
                    oneRoundRaceResultData.MergeCarFinalTimeData(oneCarData);
                }
            }
        }
        oneRoundRaceResultData.UpdateCarsRank();
        return oneRoundRaceResultData;
    }
    private void UpdateBestDriverName()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        string name = oneRoundRaceResultData.OneCarRaceResultDatas[0].CarName;
        BestDriverName.GetComponent<TextMeshProUGUI>().text = "Best driver:" + name;
    }
    private void UpdateWorstDriverName()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        string name = oneRoundRaceResultData.OneCarRaceResultDatas[oneRoundRaceResultData.OneCarRaceResultDatas.Count - 1].CarName;
        WorstDriverName.GetComponent<TextMeshProUGUI>().text = "Worst driver:" + name;
    }
    private void UpdateBestDriverTime()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        float time = oneRoundRaceResultData.OneCarRaceResultDatas[0].FinalTime;
        BestDriverTime.GetComponent<TextMeshProUGUI>().text = "Best time:" + time.ToString("f3");
    }
    private void UpdateWorstDriverTime()
    {
        OneRoundRaceResultData oneRoundRaceResultData = GetMergeRoundData();
        float time = oneRoundRaceResultData.OneCarRaceResultDatas[oneRoundRaceResultData.OneCarRaceResultDatas.Count - 1].FinalTime;
        WorstDriverTime.GetComponent<TextMeshProUGUI>().text = "Worst time:" + time.ToString("f3");
    }
    #endregion

    #region ImageUpdate
    [HideInInspector]
    public string folderPath = "SaveData/MapScreenShots";
    public Image[] MapScreenShots; // 这里假设您有一个名为image1的Image数组

    public void UpdateMapScreenShots()
    {
        LoadSpritesAndAssign();
    }
    private void LoadSpritesAndAssign()
    {
        string[] filePaths = Directory.GetFiles(Application.dataPath + "/Resources/" + folderPath, "*.jpg");
        List<Sprite> sprites = new List<Sprite>();

        /*foreach (string filePath in filePaths)
        {
            // 异步加载PNG图片为Texture2D
            Texture2D texture = new Texture2D(2, 2);
            byte[] imageData = File.ReadAllBytes(filePath);
            yield return texture.LoadImage(imageData);

            // 创建Sprite并赋值
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            sprites.Add(sprite);
        }*/
        CameraScreenshot screenshot = FindObjectOfType<CameraScreenshot>();
        foreach(var texture in screenshot.ScreenShots)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            sprites.Add(sprite);
        }
        // 将Sprite赋给image1数组的元素
        for (int i = 0; i < MapScreenShots.Length; i++)
        {
            if (i < sprites.Count)
            {
                MapScreenShots[i].sprite = sprites[i];
            }
            else
            {
                // 如果图片数量不足，可以设置一个默认的Sprite或者做一些其他处理
                Debug.LogWarning("Not enough images to assign to image1 array.");
                break;
            }
        }
        screenshot.ScreenShots.Clear();
    }
    
    #endregion
}
