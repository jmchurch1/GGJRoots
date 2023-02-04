using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private List<Tile> _tiles = new List<Tile>();
    // initialize a 100x100 grid
    // 0: dirt, 1: no dirt, 2: grass, 3: sky
    private GridSpot[,] _grid = new GridSpot[100,100];
    
    private void Awake()
    {
        InitializeGrid();
        UpdateTilemap();
    }

    private void InitializeGrid()
    {
        // use GetLength(x) instead of Length, Length returns size of entire array
        // Set the grid spots that want to be dirt, no dirt, grass, or sky
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                // for the first two layers, i < 2, set to air for the grid
                if (j > 98)
                {
                    _grid[i, j] = new GridSpot(3);
                }
                // one layer of grass
                else if (j == 97)
                {
                    _grid[i, j] = new GridSpot(2);
                }
                // the rest will be dirt
                else
                {
                    _grid[i, j] = new GridSpot(0);
                }
            }
        }

        // we also want a small spot where the player is starting, in the top middle of the map
        for (int i = _grid.GetLength(1) / 2 - 3; i < _grid.GetLength(1) / 2; i++)
        {
            for (int j = 93; j < 97; j++)
            {
                _grid[i, j].SetSpotType(2);
            }
        }
    }

    // https://stackoverflow.com/questions/54888987/how-to-set-tile-in-tilemap-dynamically
    private void UpdateTilemap()
    {
        // Set the grid spots that want to be dirt, no dirt, grass, or sky
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                int spotType = _grid[i, j].GetSpotType();
                Vector3Int pos = new Vector3Int(i, j, 0);
                _tilemap.SetTile(pos, _tiles[spotType]);
            }
        }
    }
}