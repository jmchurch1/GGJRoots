using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Ant : MonoBehaviour
{

    public int health = 30;
    
    void Awake() {
        
    }

    void Start()
    {
        GetComponent<EnemyMovement>().OnEnemyDigEvent += OnAntDig;
    }
    void OnAntDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
        grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
    }

    void Update()
    {
       if(health <= 0) {

            Destroy(this.gameObject);

        }
    }
}
