using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public CustomToggle toggleButton_Remove;

    [SerializeField]
    public CustomToggle toggleButton_Rotate;

    [SerializeField]
    public CustomToggle toggleTerrain;

    [SerializeField]
    public Text currencyText;

    internal void UpdateCurrency(int currency)
    {
        currencyText.text = currency.ToString();
    }
}
