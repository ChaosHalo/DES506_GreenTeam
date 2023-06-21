using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    private static MyGameManager instance;

    public static MyGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                if (instance == null)
                {
                    instance = new MyGameManager();
                }
            }
            return instance;
        }
    }
    // 当前车辆名次
    public int Rank = 1;
    // 车辆总数
    public int CarTotalNum;
    // Start is called before the first frame update
    void Start()
    {
        CarTotalNum = GameObject.FindGameObjectsWithTag("Car").Length;
    }
    /// <summary>
    /// 获取排名
    /// </summary>
    /// <returns></returns>
    public int GetRank()
    {
        int res = Rank;
        Rank++;
        //if (Rank > CarTotalNum) Rank = 1;
        return res;
    }
    // Update is called once per frame
    void Update()
    {

    }
}