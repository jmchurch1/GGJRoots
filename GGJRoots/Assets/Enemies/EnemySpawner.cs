using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject wormPrefab;
    [SerializeField] GameObject antPrefab;
    GridMap gridMap;

    [SerializeField] float[] waveDurationsSec = { 30.0f, 45.0f, 60.0f };
    [SerializeField] int[] numEnemiesPerWave = { 10, 20, 30 };

    // Start is called before the first frame update
    void Start()
    {
        gridMap = GridMap.instance;
        StartCoroutine(Waves());
    }
    IEnumerator Waves() {
        for (int i = 0; i < waveDurationsSec.Length; i++) {
            Debug.Log("Starting wave " + (i+1));
            StartCoroutine(Wave(i));
            yield return new WaitForSeconds(waveDurationsSec[i]);
        }
    }
    IEnumerator Wave(int waveNumber) {
        for (int i = 0; i < numEnemiesPerWave[waveNumber]; i++) {
            SpawnRandomEnemyAtRandomPos();
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        }
    }

    GameObject GetRandomEnemyPrefab() {
        int rand = Random.Range(0, 2);
        if (rand == 0) {
            return wormPrefab;
        }
        else if (rand == 1) {
            return antPrefab;
        }
        return null;
    }

    Vector2Int GetRandomCellForEnemySpawn() {
        int tilemapHeight = gridMap.GetGrid().GetLength(0); // num rows
        int tilemapWidth = gridMap.GetGrid().GetLength(1); // num cols
        // NOTE: tilemaps have origin at BOTTOM LEFT
        int side = Random.Range(0, 3);
        if (side == 0) { // bottom
            int cellX = Random.Range(0, tilemapWidth-1);
            return new Vector2Int(cellX, 0);
        }
        else if (side == 1) { // left wall
            int cellY = Random.Range(0, tilemapHeight-1  - 10);
            return new Vector2Int(0, cellY);
        }
        else if (side == 2) { // right wall
            int cellY = Random.Range(0, tilemapHeight-1  - 10);
            return new Vector2Int(tilemapWidth-1, cellY);
        }
 
        Debug.Log("GetRandomCellForEnemySpawn shouldn't reach this point!");
        return new Vector2Int(-1, -1);
    }
    void SpawnRandomEnemyAtRandomPos() {
        Vector2Int randCell = GetRandomCellForEnemySpawn();
        //Debug.Log(randCell);
        Vector3 cellWorldSpace = gridMap.TilemapCellToWorldPos(new Vector3Int(randCell.x, randCell.y, 0));
        //Debug.Log(cellWorldSpace);
        GameObject randomEnemy = GetRandomEnemyPrefab();
        Instantiate(randomEnemy, cellWorldSpace, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
