using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Worm : MonoBehaviour
{

    public int health = 60;
    
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
        if(health <= 0) {

            Destroy(this.gameObject);

        }
    }
}
