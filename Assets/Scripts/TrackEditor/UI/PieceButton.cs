using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceButton : MonoBehaviour
{
    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private Text costText;

    [SerializeField]
    private int ID;

    private void Start()
    {
        costText.text = "£"+database.objectsData[ID].cost.ToString();
    }
}
