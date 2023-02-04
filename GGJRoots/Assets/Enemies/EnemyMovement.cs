using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    GridMap grid;
    AStarPathfinder<GridSpot> pathfinder;
    Stack<Vector2Int> pathToGoal;

    bool isMoving = false;

    [SerializeField] float waitBeforeMoveToNextCell = 0.6f;

    private void Awake() {
        grid = GridMap.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new AStarPathfinder<GridSpot>(grid.GetGrid().GetLength(0), grid.GetGrid().GetLength(1), 
            (GridSpot[,] grid, int row, int col) => {
                SpotType cellType = grid[row, col].GetSpotType();
                return cellType == SpotType.Dirt || cellType == SpotType.NoDirt || cellType == SpotType.Goal;
            }
        );
    }

    Vector2Int GetGoalCell() {
        Vector2Int goal = grid.GetGoalCell();
        return goal;
    }
    // BUG: Ants moving left or down go really fast, but up or right move one at a time
    IEnumerator MoveToCell(Vector2Int cell) {
        isMoving = true;
        Vector3 positionToMoveTo = grid.TilemapCellToWorldPos(new Vector3Int(cell.x, cell.y, 0));
        // every frame move a bit closer to the destination
        transform.position = positionToMoveTo;
        yield return new WaitForSeconds(waitBeforeMoveToNextCell);
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) {
            Vector3 currentTilemapCell = grid.WorldPosToTilemapCell(transform.position);
            Vector2Int currentTilemapCellV2I = new Vector2Int((int)currentTilemapCell.x, (int)currentTilemapCell.y);
            pathToGoal = pathfinder.aStarSearch(grid.GetGrid(), currentTilemapCellV2I, GetGoalCell());
            if (pathToGoal.Count > 0) {
                //Debug.Log("Path length " + pathToGoal.Count);
                pathToGoal.Pop(); // this is our current position, don't care
                Vector2Int destinationCell = pathToGoal.Pop();
                grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
                StartCoroutine(MoveToCell(destinationCell));
            }
        }
    }
}