using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    UIManager _UI;

    private int worms = 2;
    private int ants = 2;
    private int moles = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerMovement>().OnPlayerDigEvent += OnPlayerDig;
        _UI = UIManager.instance;
        _UI.SetWormAmount(worms);
        _UI.SetAntAmount(ants);
        _UI.SetMoleAmount(moles);
    }


    void OnPlayerDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
            grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
        }
}
