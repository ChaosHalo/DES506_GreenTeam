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
    private TMP_Text costText;

    [SerializeField]
    private int ID;

    private void Start()
    {
        costText.text = database.objectsData[ID].cost.ToString();

        if (database.objectsData[ID].cost == 0)
            costText.text = "FREE";
    }
}
