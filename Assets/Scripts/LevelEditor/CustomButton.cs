using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    private PlacementSystem placementSystem;

    public int ID;

    private void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
        placementSystem.StartPlacement(ID);
    }
}
