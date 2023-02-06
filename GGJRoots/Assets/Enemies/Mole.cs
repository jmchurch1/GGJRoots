using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Mole : MonoBehaviour
{
    AudioSource audioSource;

    public int health = 90;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyMovement>().OnEnemyDigEvent += OnMoleDig;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("playAudio", 2f, 5f);
    }

    void OnMoleDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
        // horz
        if ((destinationCell - currentCell).y == 0) {
            grid.SetCell(destinationCell.x, destinationCell.y-1, new GridSpot(SpotType.NoDirt));
            grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
            grid.SetCell(destinationCell.x, destinationCell.y+1, new GridSpot(SpotType.NoDirt));
        }
        // vert
        else if ((destinationCell - currentCell).x == 0) {
            grid.SetCell(destinationCell.x-1, destinationCell.y, new GridSpot(SpotType.NoDirt));
            grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
            grid.SetCell(destinationCell.x+1, destinationCell.y, new GridSpot(SpotType.NoDirt));
        }
    }

    // Update is called once per frame
    void Update()
    {
      if(health <= 0) {

            Destroy(this.gameObject);

        }
    }

    void playAudio()
    {
        audioSource.Play();
    }
}
