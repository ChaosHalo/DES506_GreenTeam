using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    private int currency = 9999;

    [SerializeField]
    private UIManager uiManager;

    private void Start()
    {
        uiManager.UpdateCurrency(currency);
    }

    // return true if enough currency to make purchase
    internal bool MakePurchase(int cost)
    {
        // don't allow negative cost
        if (cost < 0) return false;

        // can afford
        if(CanAfford(cost))
        {
            currency -= cost;
            uiManager.UpdateCurrency(currency);
            return true;
        }

        // cannot afford
        return false;
    }

    internal bool CanAfford(int cost)
    {
        // don't allow negative cost
        if (cost < 0) return false;

        if (currency - cost > 0)
            return true;
        else
            return false;
    }

    internal void RefundCost(int cost)
    {
        // don't allow negative cost
        if (cost < 0) return;

        currency += cost;
        uiManager.UpdateCurrency(currency);
    }
}
