using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Mole : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyMovement>().OnEnemyDigEvent += OnMoleDig;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("playAudio", 2f, 5f);
    }

    void OnMoleDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
        // TODO: only dig 3x1 in direction im going
        /*for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                grid.SetCell(destinationCell.x+i, destinationCell.y+j, new GridSpot(SpotType.NoDirt));
            }
        }*/
        grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
        
 
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void playAudio()
    {
        audioSource.Play();
    }
}
