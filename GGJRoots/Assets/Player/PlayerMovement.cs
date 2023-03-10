using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Tilemap dirtTilemap;
    [SerializeField] private Animator _animator;

    private GridMap grid;
    private Vector3 _homeSpace;
    private Vector2Int currentCell;
    private float moveWaitTime = .3f;

    private bool _dead = false;
    private bool isMoving = false;

    // dig event
    public delegate void OnPlayerDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell);
    public OnPlayerDig OnPlayerDigEvent = null;

    [SerializeField] private GameObject[] _towerPrefabs;

    private int _selectedTower = 0;

    // Start is called before the first frame update
    void Start()
    {
        grid = GridMap.instance;
        _homeSpace = transform.position;
        currentCell = new Vector2Int((int)_homeSpace.x, (int)_homeSpace.y);

    }

    void Update()
    {
        placeTower();
        GetInput();

        // needs this?????
        //Debug.Log(_dead);
    }


    void GetInput() {
        // set movement animations
        SetAnimations();
        GetDirection();
        SetTowerToBePlaced();
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Player>().Kill();
        }
    }
    
    private void GetDirection()
    {
        if (isMoving) return;
        if (Input.GetKey(KeyCode.W))
        {
            StartCoroutine(MoveToCell(currentCell, currentCell + new Vector2Int(0, 1)));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(MoveToCell(currentCell, currentCell + new Vector2Int(-1, 0)));
        }
        else if (Input.GetKey(KeyCode.S))
        { 
            StartCoroutine(MoveToCell(currentCell, currentCell + new Vector2Int(0, -1)));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            StartCoroutine(MoveToCell(currentCell, currentCell + new Vector2Int(1, 0)));
        }
    }

    private void SetTowerToBePlaced() {

        if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)) {

            _selectedTower = 0;

        } else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)) {
        
            _selectedTower = 1;
        
        } else if(Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)) {

            _selectedTower = 2;
            
        }

    }
    private void SetAnimations()
    {
        float vertInput = Input.GetAxisRaw("Vertical");
        float horzInput = Input.GetAxisRaw("Horizontal");

        if (horzInput > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (horzInput < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (vertInput != 0)
        {
            _animator.SetBool("Walking", false);
            _animator.SetBool("Flying", true);
        }
        else
        {
            _animator.SetBool("Walking", true);
            _animator.SetBool("Flying", false);
        }

        Vector2 input = new Vector2(horzInput, vertInput);

        if (input == Vector2.zero)
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("Walking", false);
            _animator.SetBool("Flying", false);
        }
        else
        {
            _animator.SetBool("Idle", false);
        }
    }

    IEnumerator MoveToCell(Vector2Int currentCell, Vector2Int cell)
    {
        isMoving = true;

        SpotType cellSpotType = grid.GetGrid()[cell.x, cell.y].GetSpotType();
        bool noMove = false;
        // stop moving if player is trying to get onto impassable terrain
        if ((cellSpotType != SpotType.Dirt && cellSpotType != SpotType.NoDirt) || grid.GetGrid()[cell.x, cell.y] == null || _dead)
        {
            noMove = true;
        }

        if (!noMove)
        {
            Vector3 positionToMoveTo = grid.TilemapCellToCenteredWorldPos(new Vector3Int(cell.x, cell.y, 0));

            if (OnPlayerDigEvent != null && cellSpotType == SpotType.Dirt)
            {
                _animator.SetBool("Digging", true);
                // dig time
                yield return new WaitForSeconds(1f);
                OnPlayerDigEvent(grid, currentCell, cell);
            }
            yield return new WaitForSeconds(moveWaitTime);
            transform.position = positionToMoveTo;
            // set the current cell so that the player can continue to move
            this.currentCell = cell;
            _animator.SetBool("Digging", false);
            isMoving = false;
        }
    }

    public IEnumerator Respawn()
    {
        _dead = true;
        _animator.SetBool("Dead", _dead);
        yield return new WaitForSeconds(4f);

        transform.position = _homeSpace;
        currentCell = new Vector2Int((int)_homeSpace.x, (int)_homeSpace.y);
        GetComponent<Player>().ResetHealth();
    }

    public void SetDead(bool dead)
    {
        Debug.Log("Set Dead: " + dead);
        _dead = dead;
        _animator.SetBool("Dead", _dead);
    }

    void placeTower()
    {
        Vector3Int playerGridPos = new Vector3Int(currentCell.x, currentCell.y, 0);
        
        Vector3Int[] surroundingPoints = new Vector3Int[8];

        surroundingPoints[0] = playerGridPos + Vector3Int.up + Vector3Int.left;
        surroundingPoints[1] = playerGridPos + Vector3Int.up;
        surroundingPoints[2] = playerGridPos + Vector3Int.up + Vector3Int.right;
        surroundingPoints[3] = playerGridPos + Vector3Int.left;
        surroundingPoints[4] = playerGridPos + Vector3Int.right;
        surroundingPoints[5] = playerGridPos + Vector3Int.down + Vector3Int.left;
        surroundingPoints[6] = playerGridPos + Vector3Int.down;
        surroundingPoints[7] = playerGridPos + Vector3Int.down + Vector3Int.right;

        /* Code for radial selection below from/based off: https://www.youtube.com/watch?v=l_1NLtf6c0I */

            Vector2 selectionLoc = new Vector2(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2));

            Vector3Int selectedSpawnPoint = Vector3Int.back;

            selectionLoc.Normalize();

            if(selectionLoc != Vector2.zero) {

                float angle = Mathf.Atan2(selectionLoc.x, selectionLoc.y) / Mathf.PI;

                angle *= 180;

                angle += 45f;

                if(angle < 0) angle += 360;

                //Debug.Log(angle);

                if(angle >= 0 && angle < 45) {
                
                    selectedSpawnPoint = surroundingPoints[0];//Top Left Cell

                } else if(angle >= 45 && angle < 90) {
                
                    selectedSpawnPoint = surroundingPoints[1]; //Top Middle Cell
                    
                } else if(angle >= 90 && angle < 135) {
                
                    selectedSpawnPoint = surroundingPoints[2]; //Top Right Cell
                    
                } else if(angle >= 135 && angle < 180) {
                
                    selectedSpawnPoint = surroundingPoints[4]; //Middle Right Cell
                    
                } else if(angle >= 180 && angle < 225) {
                
                    selectedSpawnPoint = surroundingPoints[7]; //Bottom Right Cell
                    
                } else if(angle >= 225 && angle < 270) {
                
                    selectedSpawnPoint = surroundingPoints[6]; //Bottom Middle Cell
                    
                } else if(angle >= 270 && angle < 315) {
                
                    selectedSpawnPoint = surroundingPoints[5]; //Bottom Left Cell
                    
                } else if(angle >= 315 && angle < 360) {
                
                    selectedSpawnPoint = surroundingPoints[3]; //Middle Left Cell
                    
                }

                    
            }

            //ends here

            //Debug.Log(selectedSpawnPoint);

            GridSpot gs = GridMap.instance.GetGrid()[selectedSpawnPoint.x, selectedSpawnPoint.y];

            if(gs != null) {

                if(gs.GetSpotType() == SpotType.NoDirt && gs.GetTowerStatus() == false && Input.GetKeyDown(KeyCode.Mouse0)) 
                {

                    Instantiate(_towerPrefabs[_selectedTower], selectedSpawnPoint+Vector3Int.right, Quaternion.identity);

                }

            }


        

        
    }

}
