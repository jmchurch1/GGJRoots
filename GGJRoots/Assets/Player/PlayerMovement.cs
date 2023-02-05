using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    private Tilemap dirtTilemap;

    public float health;

    public GameObject _sprinklerPrefab;

    // Start is called before the first frame update
    void Start()
    {

    dirtTilemap = GameObject.Find("dirtTilemap").GetComponent<Tilemap>();

    health = 30;

    }

    Vector2 GetInput() {
        float vertInput = Input.GetAxisRaw("Vertical");
        float horzInput = Input.GetAxisRaw("Horizontal");
        Vector2 input = new Vector2(horzInput, vertInput);
        return input;
    }

    // Update is called once per frame
    void Update()
    {

        placeTower();

        if(health <= 0) {

            Destroy(this);

        }
    }

    void placeTower()
    {
        
        /* Code for radial seleciton below from/based off: https://www.youtube.com/watch?v=l_1NLtf6c0I */

            Vector2 selectionLoc = new Vector2(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2));

            Vector3Int selectedSpawnPoint = Vector3Int.back;

            selectionLoc.Normalize();

            if(selectionLoc != Vector2.zero) {

                float angle = Mathf.Atan2(selectionLoc.x, selectionLoc.y) / Mathf.PI;

                angle *= 180;

                angle += 45f;

                if(angle < 0) angle += 360;

                if(angle >= 0 && angle < 45) {
                
                    //Top Left Cell

                } else if(angle >= 45 && angle < 90) {
                
                     //Top Middle Cell
                    
                } else if(angle >= 90 && angle < 135) {
                
                    //Top Right Cell
                    
                } else if(angle >= 135 && angle < 180) {
                
                     //Middle Left Cell
                    
                } else if(angle >= 180 && angle < 225) {
                
                     //Bottom Left Cell
                    
                } else if(angle >= 225 && angle < 270) {
                
                     //Bottom Middle Cell
                    
                } else if(angle >= 270 && angle < 315) {
                
                     //Bottom Right Cell
                    
                } else if(angle >= 315 && angle < 360) {
                
                     //Middle Right Cell
                    
                }

                    
            }

            //ends here

            //GridSpot gs = GridMap.instance.GetGrid()[selectedSpawnPoint.x, selectedSpawnPoint.y];

            // if(gs != null) {

            //     if(gs.GetSpotType() == SpotType.NoDirt && gs.GetTowerStatus() == false) 
            //     {

            //         Instantiate(_sprinklerPrefab, spawnPoint);

            //     }

            // }


        

        
    }

}
