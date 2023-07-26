using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockableButton : MonoBehaviour
{
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private ObjectsDatabaseSO database;
    private int cost;
    [SerializeField] internal int ID;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image otherImage;
    [SerializeField] private GameObject otherCost;

    internal bool isLocked = true;

    private void Start()
    {
        cost = database.objectsData[ID].unlockCost;
        costText.text = cost.ToString();
    }

    public void TryUnlock()
    {
        if (cost == 0)
            return;

        if (currencyManager.CanAfford(cost))
            Unlock();
    }

    private void Unlock()
    {
        isLocked = false;
        otherImage.enabled = true;
        otherCost.SetActive(true);
        currencyManager.MakePurchase(cost);
        gameObject.SetActive(false);
    }

    internal void Lock()
    {
        isLocked = true;
        otherImage.enabled = false;
        otherCost.SetActive(false);
        gameObject.SetActive(true);
    }
}
