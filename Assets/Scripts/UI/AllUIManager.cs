using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Missions;
using Lovatto.MiniMap;

public class AllUIManager : MonoBehaviour
{
    [Header("Events")]
    public MissionEvent onRaceEndTime;
    public MissionEvent onRaceEndCarInfo;
    public void TryAgain()
    {
        ClearAllCars();
        if (MyGameManager.instance.Map != null)
        {
            //Destroy(MyGameManager.instance.Map);
        }
        // 游戏轮次增加
        MyGameManager.instance.GameRound++;

        // events
        double raceTime = MyGameManager.instance.GetRaceManager().GetRaceTime();
        onRaceEndTime.Raise(this, raceTime);
        onRaceEndCarInfo.Raise(MyGameManager.instance.GetCarInfoSerach(), null);

        // change state
        MyGameManager.instance.GetSceneManager().ChangeToState_CheckRacers();

        // win currency
        FindObjectOfType<CurrencyManager>().AddWinCurrency();

    }
    private void ClearAllCars()
    {
        CarManager[] CarManager = FindObjectsOfType<CarManager>();
        // 清除小地图UI
        bl_MiniMapIcon[] bl_MiniMapIcons = FindObjectsOfType<bl_MiniMapIcon>();
        if (bl_MiniMapIcons.Length > 0)
        {
            Debug.Log("已清除小地图UI");
        }
        foreach (var i in bl_MiniMapIcons)
        {
            Destroy(i.gameObject);
        }
        foreach (var Car in CarManager)
        {
            Destroy(Car.gameObject);
        }
    }
}
