﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
using UnityEngine.SceneManagement;
public class SpeedMeasurementSystemsTest : MonoBehaviour
{
    public SpeedMeasurementPoint p1;
    public SpeedMeasurementPoint p2;
    public RaceManager SingleRaceManager;
    public struct Info
    {
        public string Name;
        public float Speed;
        public Vector3 position;
        public float Time;
    }
    public List<Info> Infos = new();
    //开始
    // Start is called before the first frame update
    void Start()
    {
        SingleRaceManager.StartRace();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMatch();
        if (Input.GetKeyDown(KeyCode.P))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 使用索引重新加载当前场景
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
    private float GetDis(Info info1, Info info2)
    {
        return Vector3.Distance(info1.position, info2.position);
    }
    private float GetTimeDif(Info info1, Info info2)
    {
        return Mathf.Abs(info1.Time - info2.Time);
    }
    private float GetAcceleration(Info info1, Info info2)
    {
        return Mathf.Abs(info1.Speed - info2.Speed) / GetTimeDif(info1, info2);
    }
    private void CheckMatch()
    {
        for(int i = 0; i < Infos.Count; i++)
        {
            if(i != Infos.Count - 1)
            {
                for (int j = i + 1; j < Infos.Count; j++)
                {
                    if(Infos[i].Name == Infos[j].Name)
                    {
                        Print(Infos[i], Infos[j]);
                        Infos.Remove(Infos[i]);
                        Infos.Remove(Infos[j - 1]);
                        return;
                    }
                }
            }
        }
    }
    private void Print(Info info1, Info info2)
    {
        Debug.Log("Car Name: " + info1.Name + 
            "\nCar Speed1: " + info1.Speed.ToString("F2") +
            "\nCar Speed2: " + info2.Speed.ToString("F2") +
            "\nDistance: " + GetDis(info1, info2).ToString("F2") +
            "\nTime: " + GetTimeDif(info1, info2).ToString("F2") +
            "\nAcceleration" + GetAcceleration(info1, info2).ToString("F2")
            );
    }
}
