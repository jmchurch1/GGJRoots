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

    private GridSpot _goalSpot;
    [SerializeField] private Vector2Int _goalCell;


    public static GridMap instance;
    
    private void Awake()
    {
        instance = this;

        InitializeGrid();
        UpdateTilemap();
    }

    public GridSpot[,] GetGrid() { return _grid; }

    public void SetCell(int row, int col, GridSpot newCell) {
        _grid[row, col] = newCell;
        Vector3Int pos = new Vector3Int(row, col, 0);
        SpotType spotType = newCell.GetSpotType();
        _tilemap.SetTile(pos, _tiles[(int)spotType]);
    }
    public Vector3Int WorldPosToTilemapCell(Vector3 worldPos) {
        return _tilemap.WorldToCell(worldPos);
    }
    public Vector3 TilemapCellToWorldPos(Vector3Int cell) {
        return _tilemap.CellToWorld(cell);
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
                    _grid[i, j] = new GridSpot(SpotType.Sky);
                }
                // one layer of grass
                else if (j == 97)
                {
                    _grid[i, j] = new GridSpot(SpotType.Grass);
                }
                // the rest will be dirt
                else
                {
                    _grid[i, j] = new GridSpot(SpotType.Dirt);
                }
            }
        }

        // we also want a small spot where the player is starting, in the top middle of the map
        for (int i = _grid.GetLength(1) / 2 - 3; i < _grid.GetLength(1) / 2; i++)
        {
            for (int j = 93; j < 97; j++)
            {
                _grid[i, j].SetSpotType(SpotType.NoDirt);
            }
        }


        _grid[_goalCell.x, _goalCell.y].SetSpotType(SpotType.Goal);
        _goalSpot = _grid[_goalCell.x, _goalCell.y];
    }

    // https://stackoverflow.com/questions/54888987/how-to-set-tile-in-tilemap-dynamically
    private void UpdateTilemap()
    {
        // Set the grid spots that want to be dirt, no dirt, grass, or sky
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                SpotType spotType = _grid[i, j].GetSpotType();
                Vector3Int pos = new Vector3Int(i, j, 0);
                _tilemap.SetTile(pos, _tiles[(int)spotType]);
            }
        }
    }

    public GridSpot GetGoalSpot()
    {
        return _goalSpot;
    }
    public Vector2Int GetGoalCell() {
        return _goalCell;
    }
}
