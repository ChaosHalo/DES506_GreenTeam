using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Missions;


public class AllUIManager : MonoBehaviour
{
    [Header("Events")]
    public MissionEvent onRaceEndTime;
    public MissionEvent onRaceEndCarInfo;
    public void TryAgain()
    {
        EventsHandle();
        ChangeState();
        WinCurrency();
    }
    public void GameRoundIncrease()
    {
        // 游戏轮次增加
        MyGameManager.instance.GameRound++;
    }
    protected virtual void EventsHandle()
    {
        // events
        double raceTime = MyGameManager.instance.GetRaceManager().GetRaceTime();
        onRaceEndTime.Raise(this, raceTime);
        onRaceEndCarInfo.Raise(MyGameManager.instance.GetCarInfoSerach(), null);
    }
    protected virtual void ChangeState()
    {
        // change state
        MyGameManager.instance.GetSceneManager().ChangeToState_Build();
    }
    protected virtual void WinCurrency()
    {
        // win currency
        MyGameManager.instance.GetCurrencyManager().AddWinCurrency();
    }
    
}
