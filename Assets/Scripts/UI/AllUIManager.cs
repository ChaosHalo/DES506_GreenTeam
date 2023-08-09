using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Missions;
using Cinemachine;

public class AllUIManager : MonoBehaviour
{
    public void TryAgain()
    {
        FindObjectOfType<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        FindObjectOfType<CinemachineBrain>().m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
        EventsHandle();
        WinCurrency();
        ChangeState();
    }
    public void GameRoundIncrease()
    {
        // 游戏轮次增加
        MyGameManager.instance.GameRound++;
    }
    protected virtual void EventsHandle()
    {
        // events
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
