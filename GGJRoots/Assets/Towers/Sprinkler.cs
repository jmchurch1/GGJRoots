using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sprinkler : MonoBehaviour
{
    private int _health = 10;

    private int _maxHP;

    // Start is called before the first frame update
    void Start()
    {

        _maxHP = _health;

        InvokeRepeating("Decay", 1.0f, 1.0f);

    }
        

    // Update is called once per frame
    void Update()
    {
        
        if(_health <= 0) {
            Destroy(this.gameObject);
        }

    }

    void Decay() 
    {

        _health--;

    }

    public void Repair() 
    {

        _health = _maxHP;

    }

    void OnTriggerEnter2D(Collider2D collision) {

        if(collision.gameObject.GetComponent<EnemyMovement>() != null)
            collision.gameObject.GetComponent<EnemyMovement>().waitBeforeMoveToNextCell = collision.gameObject.GetComponent<EnemyMovement>().waitBeforeMoveToNextCell + 0.3f; //*Assumes enemies will have a speed parameter

        Debug.Log("colliding");

    }

    void OnTriggerExit2D(Collider2D collision) {

        if(collision.gameObject.GetComponent<EnemyMovement>() != null)
            collision.gameObject.GetComponent<EnemyMovement>().waitBeforeMoveToNextCell = collision.gameObject.GetComponent<EnemyMovement>().waitBeforeMoveToNextCell - 0.3f;

    }

}
