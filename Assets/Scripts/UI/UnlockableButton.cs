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
    [SerializeField] private int ID;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image otherImage;
    [SerializeField] private GameObject otherCost;

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
        {
            otherImage.enabled = true;
            otherCost.SetActive(true);
            currencyManager.MakePurchase(cost);
            gameObject.SetActive(false);
        }
    }
}
