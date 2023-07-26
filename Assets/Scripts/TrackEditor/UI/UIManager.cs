using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CustomToggle toggleButton_Remove;

    public CustomToggle toggleButton_Rotate;

    public CustomToggle toggleTerrain;

    public TMP_Text currencyText;


    [Header("Floating Currency")]
    public GameObject spentCurrencyPrefab;
    public GameObject spentCurrenyParent;
    [Header("Floating Start Race")]
    public GameObject startRacePrefab;
    public GameObject startRaceParent;
    [Header("Error Messages")]
    public string[] errorMessages;

    internal void UpdateCurrency(int currency)
    {
        currencyText.text = currency.ToString();
    }

    internal void OnCurrencyModified(int modificationAmount)
    {
        GameObject newObj = Instantiate(spentCurrencyPrefab);
        newObj.transform.SetParent(spentCurrenyParent.transform, false);
        newObj.transform.position = Input.mousePosition;
        newObj.GetComponentInChildren<FloatingCurrencyText>().SetupVariables(modificationAmount);
    }

    internal void OnStartRacePressed(int errorIndex)
    {
        GameObject newObj = Instantiate(startRacePrefab);
        newObj.transform.SetParent(startRaceParent.transform, false);
        newObj.transform.position = Input.mousePosition;
        newObj.GetComponentInChildren<FloatingStartRaceText>().SetupVariables(errorMessages[errorIndex]);
    }
}
