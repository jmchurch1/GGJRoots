using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private List<Tile> _tiles = new List<Tile>();
    [SerializeField] Vector2Int gridDimensions;

    // grid to hold all tiles
    private GridSpot[,] _grid;

    private GridSpot _goalSpot;
    [SerializeField] private Vector2Int _goalCell;


    public static GridMap instance;
    
    private void Awake()
    {
        _grid = new GridSpot[gridDimensions.x,gridDimensions.y];
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
    public Vector3 TilemapCellToCenteredWorldPos(Vector3Int cell) {
        return _tilemap.GetCellCenterWorld(cell);
    }

    private void InitializeGrid()
    {
        int numRows = _grid.GetLength(0);
        int numCols = _grid.GetLength(1);
        int numAirLayers = 2;
        int grassLayer = numAirLayers+1; // 1 grass layer after air layers
        // use GetLength(x) instead of Length, Length returns size of entire array
        // Set the grid spots that want to be dirt, no dirt, grass, or sky
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // TODO(?): why are the cols/rows swapped
                // for the first few layers, set to air for the grid
                if (j >= numCols-numAirLayers)
                {
                    _grid[i, j] = new GridSpot(SpotType.Sky);
                }
                // one layer of grass
                else if (j >= numCols-grassLayer)
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

        int playerStartAreaWidth = 3;
        int playerStartAreaHeight = 4;
        // we also want a small spot where the player is starting, in the top middle of the map
        for (int i = numRows / 2 - playerStartAreaWidth; i < numRows / 2; i++)
        {
            for (int j = numCols-grassLayer-playerStartAreaHeight; j < numCols-grassLayer; j++)
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
