using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public List<int> RankList;
    public List<float> TimeForOneLapList;
    private float oneLapTime;
    private float totalTime;
    private void Start()
    {
        RankList = new();
        TimeForOneLapList = new();
    }
    private void Update()
    {
        Timer();
    }
    private void Timer()
    {
        oneLapTime += Time.deltaTime;
        totalTime += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndLine"))
        {
            OnOneLapEnd();
        }
    }
    private void OnOneLapEnd()
    {
        TimeForOneLapList.Add(Mathf.Round(oneLapTime * 1000) / 1000f);
        RankList.Add(MyGameManager.Instance.GetRank());
        oneLapTime = 0;
    }
    public void OnEnd()
    {

    }
    public void OnStart()
    {

    }
}
