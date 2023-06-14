using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private Grid grid;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);  
    }
}
