using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Worm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyMovement>().OnEnemyDigEvent += OnWormDig;
    }

    void OnWormDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
        grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
