using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Heavily derived from:
https://www.geeksforgeeks.org/a-search-algorithm/


For example usage, see Test() function at bottom of this file
*/



public class AStarPathfinder<T>
{
    public struct cell {
        public int parent_row; // parent row idx
        public int parent_col; // parent col idx
        public double f; // accumulated cost + estimated cost to dest
        public double g; // cost from src to this cell
        public double h; // estimated cost from this cell to dest
    };
    public struct pPair {
        // <f, <i, j>>
        // where f = g + h,
        // and i, j are the row and column index of that cell
        public double first;
        public Vector2Int second;
        public pPair(double first, Vector2Int second) {
            this.first = first; this.second = second;
        }
    }

    public delegate bool VoidRowColFunc(T[,] grid, int row, int col);

    public VoidRowColFunc isUnBlocked = null;
    Vector2Int dimensions = Vector2Int.zero;

    /// pass in width & height (of grid), and a delegate that returns true
    /// when the specified position in the grid is UNBLOCKED (I.E. available to move through)
    public AStarPathfinder(int width, int height, VoidRowColFunc isCellUnBlockedCB) {
        this.isUnBlocked = isCellUnBlockedCB;
        dimensions = new Vector2Int(width, height);
    }

    // calculate heuristic analysis
    double calculateHValue(int row, int col, Vector2Int dest)
    {
        // Manhattan distance
        return Mathf.Abs(row - dest.x) + Mathf.Abs(col - dest.y);
    }
    // is grid[row,col] the goal?
    bool isDestination(int row, int col, Vector2Int dest)
    {
        return row == dest.x && col == dest.y;
    }

    // is [row,col] in bounds
    bool isValid(int row, int col) {
        return (row >= 0) && (row < this.dimensions.x) && (col >= 0)
           && (col < this.dimensions.y);
    }

    /// A Function to find the shortest path between
    /// a given source cell to a destination cell according
    /// to A* Search Algorithm
    /// Returns a stack of cells (row, col) where the top of the stack is the
    /// starting node (src) and the bottom of the stack is the ending cell (dest)
    public Stack<Vector2Int> aStarSearch(T[,] grid, Vector2Int src, Vector2Int dest)
    {
        Stack<Vector2Int> ret = new Stack<Vector2Int>();
        // Early returns
        if (!isValid(src.x, src.y)) {
            Debug.Log("Source invalid\n");
            return ret;
        }
        if (!isValid(dest.x, dest.y)) {
            Debug.Log("Destination is invalid\n");
            return ret;
        }
        if (!isUnBlocked(grid, src.x, src.y)
            || !isUnBlocked(grid, dest.x, dest.y)) {
            Debug.Log("Source or destination is blocked\n");
            return ret;
        }
        if (isDestination(src.x, src.y, dest)) {
            Debug.Log("already at the destination\n");
            return ret;
        }
    
        // closed list -> represents cells that have been visited
        bool[,] closedList = new bool[dimensions.x, dimensions.y];
        
        // "details" of each cell (parent cell and some cost information)
        cell[,] cellDetails = new cell[dimensions.x, dimensions.y];

        // init cell details to "blank" values
        for (int i = 0; i < dimensions.x; i++) {
            for (int j = 0; j < dimensions.y; j++) {
                cellDetails[i, j].f = float.MaxValue;
                cellDetails[i, j].g = float.MaxValue;
                cellDetails[i, j].h = float.MaxValue;
                cellDetails[i, j].parent_row = -1;
                cellDetails[i, j].parent_col = -1;
            }
        }
    
        // Init parameters of the starting node
        int src_row = src.x; int src_col = src.y;
        cellDetails[src_row,src_col].f = 0.0;
        cellDetails[src_row,src_col].g = 0.0;
        cellDetails[src_row,src_col].h = 0.0;
        cellDetails[src_row,src_col].parent_row = src_row;
        cellDetails[src_row,src_col].parent_col = src_col;
    
        /*
        Create an open list having information as-
        <f, <i, j>>
        where f = g + h,
        and i, j are the row and column index of that cell
        Note that 0 <= i <= ROW-1 & 0 <= j <= COL-1
        */
        List<pPair> openList = new List<pPair>();
    
        // Put the starting cell on the open list and set its
        // 'f' as 0
        openList.Add(new pPair(0.0, new Vector2Int(src_row, src_col)));
    
        bool foundDest = false;
    
        while (openList.Count != 0) {
            pPair p = openList[0];
            int i = p.second.x;
            int j = p.second.y;
    
            // indicate we've visited this cell
            // (remove from open list, add to closed list)
            openList.RemoveAt(0);
            closedList[i,j] = true;
    
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i-1, j,  dest, openList, closedList, cellDetails, ref ret); // N
            if (foundDest) return ret;
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i+1, j,  dest, openList, closedList, cellDetails, ref ret); // S
            if (foundDest) return ret;
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i, j+1,  dest, openList, closedList, cellDetails, ref ret); // E
            if (foundDest) return ret;
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i, j-1,  dest, openList, closedList, cellDetails, ref ret); // W
            if (foundDest) return ret;

            /*
            // uncomment to enable diagonal moves
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i-1, j+1,  dest, openList, closedList, cellDetails, ref ret); // NE
            if (foundDest) return ret;
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i-1, j-1,  dest, openList, closedList, cellDetails, ref ret); // NW
            if (foundDest) return ret;
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i+1, j+1,  dest, openList, closedList, cellDetails, ref ret); // SE
            if (foundDest) return ret;
            foundDest = PerformAStarStepInDirection(grid,  i,j,   i+1, j-1,  dest, openList, closedList, cellDetails, ref ret); // SW
            if (foundDest) return ret;
            */

        }
    

        if (!foundDest) {
            Debug.Log("Failed to find the Destination Cell\n");
        }

        return ret;
    }

    private bool PerformAStarStepInDirection(T[,] grid, int i, int j, int dirX, int dirY, Vector2Int dest, 
                                            List<pPair> openList, bool[,] closedList,
                                            cell[,] cellDetails, ref Stack<Vector2Int> finishedPath) {
        
        // Only process this cell if this is a valid one
        if (isValid(dirX, dirY)) {
            // If the destination cell is the same as the
            // current successor
            if (isDestination(dirX, dirY, dest)) {
                // Set the Parent of the destination cell
                cellDetails[dirX, dirY].parent_row = i;
                cellDetails[dirX, dirY].parent_col = j;
                //Debug.Log("Reached destination\n");
                finishedPath = tracePath(cellDetails, dest);
                return true;
            }
            // If the successor is already on the closed
            // list or if it is blocked, then ignore it.
            // Else do the following
            else if (!closedList[dirX, dirY] && isUnBlocked(grid, dirX, dirY)) {
                double gNew = cellDetails[i,j].g + 1.0;
                double hNew = calculateHValue(dirX, dirY, dest);
                double fNew = gNew + hNew;

                // If it isn’t on the open list, add it to
                // the open list. Make the current square
                // the parent of this square. Record the
                // f, g, and h costs of the square cell
                //                OR
                // If it is on the open list already, check
                // to see if this path to that square is
                // better, using 'f' cost as the measure.
                if (cellDetails[dirX, dirY].f == float.MaxValue
                    || cellDetails[dirX, dirY].f > fNew) {
                    openList.Add(new pPair(
                        fNew, new Vector2Int(dirX, dirY)));

                    // Update the details of this cell
                    cellDetails[dirX, dirY].f = fNew;
                    cellDetails[dirX, dirY].g = gNew;
                    cellDetails[dirX, dirY].h = hNew;
                    cellDetails[dirX, dirY].parent_row = i;
                    cellDetails[dirX, dirY].parent_col = j;
                }
            }
        }

        return false;
    }

    // returns the path A* took to the destination
    // top of the stack is the starting node
    Stack<Vector2Int> tracePath(cell[,] cellDetails, Vector2Int dest)
    {
        int row = dest.x;
        int col = dest.y;
    
        Stack<Vector2Int> Path = new Stack<Vector2Int>();
    
        // start at destination cell and walk up the parents
        // pushing each to a stack (reverses order)
        while (!(cellDetails[row,col].parent_row == row
                && cellDetails[row,col].parent_col == col)) {
            Path.Push(new Vector2Int(row, col));
            int temp_row = cellDetails[row,col].parent_row;
            int temp_col = cellDetails[row,col].parent_col;
            row = temp_row;
            col = temp_col;
        }
    
        Path.Push(new Vector2Int(row, col));
        return Path;
    }

    public static void Test() {

        int[,] grid
            = { { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 0, 1, 0, 1 },
                { 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 0 },
                { 1, 0, 1, 1, 1, 1, 0, 1, 0, 0 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 },
                { 1, 1, 1, 0, 0, 0, 1, 0, 0, 1 } };
    
        // Source is the left-most bottom-most corner
        Vector2Int src = new Vector2Int(8, 0);
    
        // Destination is the left-most top-most corner
        Vector2Int dest = new Vector2Int(0, 0);
        AStarPathfinder<int> pathfinder = new AStarPathfinder<int>(9, 10, 
            // function that returns true when the cell is UNBLOCKED (clear to go through)
            (int[,] grid, int row, int col) => {
                 return grid[row,col] == 1;
            }
        );
        Stack<Vector2Int> path = pathfinder.aStarSearch(grid, src, dest);
        Debug.Log("Found valid path? " + (path.Count > 0 ? true : false));
        while (path.Count != 0) {
            Vector2Int spot = path.Pop();
            Debug.Log("-> (" + spot.x + ", " + spot.y + ")");
        }

    }
}
