using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int currencyCurrent=0;
    public int GetPlayerCurrency() { return currencyCurrent; }

    [SerializeField]
    private int currencyStart = 2000;

    [SerializeField]
    private int currencyWin = 1000;

    [SerializeField]
    private UIManager uiManager;

    [Header("Events")]
    public MissionEvent onSpendCurrency;

    private void Start()
    {
        currencyCurrent = currencyStart;
        uiManager.UpdateCurrency(currencyCurrent);
    }

    // return true if enough currency to make purchase
    internal bool MakePurchase(int cost)
    {
        // don't allow negative cost
        if (cost < 0) return false;

        // can afford
        if(CanAfford(cost))
        {
            currencyCurrent -= cost;
            uiManager.UpdateCurrency(currencyCurrent);
            onSpendCurrency.Raise(this, -cost);
            return true;
        }

        // cannot afford
        return false;
    }

    internal bool CanAfford(int cost)
    {
        // don't allow negative cost
        if (cost < 0) return false;

        if (currencyCurrent - cost >= 0)
            return true;
        else
            return false;
    }

    internal void RefundCost(int cost)
    {
        // don't allow negative cost
        if (cost < 0) return;

        currencyCurrent += cost;
        uiManager.UpdateCurrency(currencyCurrent);
        onSpendCurrency.Raise(this, cost);
    }

    internal void AddWinCurrency()
    {
        currencyCurrent += currencyWin;
        uiManager.UpdateCurrency(currencyCurrent);
    }
}
