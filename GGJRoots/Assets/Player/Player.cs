using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerMovement>().OnPlayerDigEvent += OnPlayerDig;   
    }


    void OnPlayerDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
            grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
        }
}
