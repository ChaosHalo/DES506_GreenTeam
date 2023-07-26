using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceButton : MonoBehaviour
{
    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private TMP_Text textObject;

    [SerializeField]
    private int ID;

    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TooltipDatabaseSO tooltipDatabase;


    private bool isTooltipVisible = false;
    private bool isTapHold = false;
    private float minHoldDuration = 0.3f;
    private float curHoldDuration = 0;

    private Vector3 downPos;
    private float minDistance = 10f;


    private void Update()
    {
        if (isTapHold)
        {
            curHoldDuration += Time.deltaTime;
            if (curHoldDuration >= minHoldDuration)
            {
                if (isTooltipVisible == false)
                {
                    ShowTooltip(true);
                }
            }

            if (IsBeyondMinDistance())
            {
                if (isTooltipVisible == true)
                {
                    ShowTooltip(false);
                }
            }
        }
    }

    private bool IsBeyondMinDistance()
    {
        return Vector3.Distance(downPos, Input.mousePosition) >= minDistance;
    }

    private void ShowTooltip(bool visible)
    {
        isTooltipVisible = visible;
        tooltipObject.SetActive(visible);
        textObject.text = tooltipDatabase.tooltipTexts[ID];
        curHoldDuration = 0;
    }

    public void OnTapDown()
    {
        isTapHold = true;
        downPos=Input.mousePosition;
    }

    public void OnTapUp()
    {
        isTapHold = false;
        ShowTooltip(false);
    }
}
