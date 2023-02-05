using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Tilemap dirtTilemap;
    [SerializeField] private Animator _animator;

    private Vector3 _homeSpace;
    private float _maxHealth = 30;
    private float _speed = 5f;

    private bool _dead = false;


    public float health;

    public GameObject _sprinklerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _homeSpace = transform.position;

    dirtTilemap = GameObject.Find("dirtTilemap").GetComponent<Tilemap>();

    health = 30;

    }


    void GetInput() {
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

        transform.position += new Vector3(input.x, input.y, 0) * Time.deltaTime * _speed;
    }

    // Update is called once per frame
    void Update()
    {

        // placeTower();
        GetInput();
        

        if(health <= 0 && !_dead) {
            _dead = true;
            _animator.SetBool("Dead", _dead);
            
            StartCoroutine(nameof(Respawn));
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(4f);

        transform.position = _homeSpace;
        health = _maxHealth;
        _dead = false;
    }

    void placeTower()
    {
        Vector3Int playerGridPos = dirtTilemap.WorldToCell(transform.position);
        
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

            // GridSpot gs = GridMap.instance.GetGrid()[selectedSpawnPoint.x, selectedSpawnPoint.y];

            // if(gs != null) {

            //     if(gs.GetSpotType() == SpotType.NoDirt && gs.GetTowerStatus() == false) 
            //     {

            //         Instantiate(_sprinklerPrefab, selectedSpawnPoint, Quaternion.identity);

            //     }

            // }


        

        
    }

}
