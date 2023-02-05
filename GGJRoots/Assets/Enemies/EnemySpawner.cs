using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    GridMap gridMap;
    UIManager _UI;

    [SerializeField] int[] numEnemiesPerWave = { 10, 20, 30 };
    [SerializeField] Vector2 enemySpawnIntervalRangeSec = new Vector2(5.0f, 10.0f);
    int waveNumber = 0;
    public int GetWaveNumber() {
        return waveNumber;
    }
    GameObject GetRandomEnemyPrefab() {
        int rand = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[rand];
    }
    public void StartWaves() {
        StartCoroutine(Waves());
    }
    void Start()
    {
        gridMap = GridMap.instance;
        _UI = UIManager.instance;
        Debug.Log("Starting enemy spawn waves for debugging...");
        StartWaves();
    }
    IEnumerator Waves() {
        for (int i = 0; i < numEnemiesPerWave.Length; i++) {
            Debug.Log("Starting wave " + (i+1));
            _UI.SetWaveAmount(i + 1);
            waveNumber = i;
            yield return StartCoroutine(Wave(i)); // waits for this coroutine to finish before going to the next one
            yield return StartCoroutine(CheckForAllEnemiesDead());
        }
    }
    IEnumerator Wave(int waveNumber) {
        for (int i = 0; i < numEnemiesPerWave[waveNumber]; i++) {
            SpawnRandomEnemyAtRandomPos();
            yield return new WaitForSeconds(Random.Range(enemySpawnIntervalRangeSec.x, enemySpawnIntervalRangeSec.y));
        }
    }
    IEnumerator CheckForAllEnemiesDead() {
        while (!isAllEnemiesDead()) {
            yield return new WaitForSeconds(0.5f);
        }
    }
    bool isAllEnemiesDead() {
        return transform.childCount == 0;
    }

    Vector2Int GetRandomCellForEnemySpawn() {
        int tilemapHeight = gridMap.GetGrid().GetLength(1); // num rows
        int tilemapWidth = gridMap.GetGrid().GetLength(0); // num cols
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
        Vector3 cellWorldSpace = gridMap.TilemapCellToWorldPos(new Vector3Int(randCell.x, randCell.y, 0));
        GameObject randomEnemy = GetRandomEnemyPrefab();
        Instantiate(randomEnemy, cellWorldSpace, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
